﻿// TODO fill in this information for your driver, then remove this line!
//
// ASCOM Focuser hardware class for McMullenTechFocuser
//
// Description:	 <To be completed by driver developer>
//
// Implements:	ASCOM Focuser interface version: <To be completed by driver developer>
// Author:		(XXX) Your N. Here <your@email.here>
//

using ASCOM;
using ASCOM.Astrometry.AstroUtils;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace ASCOM.McMullenTechFocuser.Focuser
{
    //
    // TODO Replace the not implemented exceptions with code to implement the function or throw the appropriate ASCOM exception.
    //

    /// <summary>
    /// ASCOM Focuser hardware class for McMullenTechFocuser.
    /// </summary>
    [HardwareClass()] // Class attribute flag this as a device hardware class that needs to be disposed by the local server when it exits.
    internal static class FocuserHardware
    {
        // Constants used for Profile persistence
        internal const string comPortProfileName = "COM Port";
        internal const string comPortDefault = "COM1";
        internal const string traceStateProfileName = "Trace Level";
        internal const string traceStateDefault = "true";

        private static string DriverProgId = ""; // ASCOM DeviceID (COM ProgID) for this driver, the value is set by the driver's class initialiser.
        private static string DriverDescription = ""; // The value is set by the driver's class initialiser.
        internal static string comPort; // COM port name (if required)
        private static bool connectedState = false; // Local server's connected state
        private static bool runOnce = false; // Flag to enable "one-off" activities only to run once.
        internal static Util utilities; // ASCOM Utilities object for use as required
        internal static AstroUtils astroUtilities; // ASCOM AstroUtilities object for use as required
        internal static TraceLogger tl; // Local server's trace logger object for diagnostic log with information that you specify


        // TODO Custom internal variables
        internal static SerialPort serialPort;
        internal static int baudRate;
        internal static string serialRxBuff = null;
        internal static string rxString = null;
        internal static bool newRX = false;
        internal static int responseTimeout = 1000;

        internal static bool moving = false;
        internal static bool hasMoved = true;

        internal static float temperature = 0;
        


        /// <summary>
        /// Initializes a new instance of the device Hardware class.
        /// </summary>
        static FocuserHardware()
        {
            try
            {
                // Create the hardware trace logger in the static initialiser.
                // All other initialisation should go in the InitialiseHardware method.
                tl = new TraceLogger("", "McMullenTechFocuser.Hardware");

                // DriverProgId has to be set here because it used by ReadProfile to get the TraceState flag.
                DriverProgId = Focuser.DriverProgId; // Get this device's ProgID so that it can be used to read the Profile configuration values

                // ReadProfile has to go here before anything is written to the log because it loads the TraceLogger enable / disable state.
                ReadProfile(); // Read device configuration from the ASCOM Profile store, including the trace state

                LogMessage("FocuserHardware", $"Static initialiser completed.");
            }
            catch (Exception ex)
            {
                try { LogMessage("FocuserHardware", $"Initialisation exception: {ex}"); } catch { }
                MessageBox.Show($"{ex.Message}", "Exception creating ASCOM.McMullenTechFocuser.Focuser", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }


        internal static void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string res = null;
            float val = 0;
            try
            { 
                serialRxBuff = serialPort.ReadLine();
            }
            catch (Exception)
            {
                LogMessage("serialPortReadCallback", "SerialPort read timeout");
                return;
            }

            int from = serialRxBuff.IndexOf("<")+1;
            int to = serialRxBuff.IndexOf(">");
            if(from != -1 && to != -1)
            {
                res = serialRxBuff.Substring(from, to - from);
            }
            else
            {
                // invalid message
                LogMessage("serialPortReadCallback", "Invalid message: " + serialRxBuff);
                return;
            }
            if(res == null || res.Length < 3)
            {
                LogMessage("serialPortReadCallback", "Invalid message: " + serialRxBuff);
                return;
            }

            LogMessage("serialPortReadCallback", "Got message: " + serialRxBuff + " " + res);

            try
            {
                if (res[0] == 'I') // Information
                {
                    if (res[1] == 'm') // Moving
                    {
                        if (!float.TryParse(res.Substring(2, res.Length-2), out val)) { throw new Exception("ValueError"); }
                        else
                        {
                            moving = val != 0;
                            if (moving)
                            {
                                hasMoved = true;
                            }
                        
                        }
                    }
                    else if(res[1] == 't')
                    {
                        if (!float.TryParse(res.Substring(2, res.Length - 2), out val)) { throw new Exception("ValueError"); }
                        else
                        {
                            temperature = val;
                        }
                    }
                    else if (res[1] == 'p')
                    {
                        if (!float.TryParse(res.Substring(2, res.Length - 2), out val)) { throw new Exception("ValueError"); }
                        else
                        {
                            focuserPosition = (int)val;
                        }
                    }
                }
                else if (res[0] == 'E') // Error
                {
                    LogMessage("serialPortReadCallback", "Error message: " + res);
                }
            }
            catch (Exception)
            {
                LogMessage("serialPortReadCallback", "Error when parsing message: " + res);
            }

            rxString = res;
            newRX = true;
            
        }



        internal static void ConnectPort()
        {
            try
            {
                serialPort = new SerialPort(comPort);
                serialPort.BaudRate = baudRate;
                serialPort.Parity = Parity.None;
                serialPort.StopBits = StopBits.One;
                serialPort.DataBits = 8;
                serialPort.Handshake = Handshake.None;
                serialPort.RtsEnable = true;
                serialPort.NewLine = "\r\n";
                serialPort.ReadTimeout = 2000;
                serialPort.WriteTimeout = 2000;
                serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);

                if (!serialPort.IsOpen)
                {
                    LogMessage("ConnectPort", "Opening port " + comPort);
                    try { serialPort.Open(); }
                    catch (Exception)
                    {
                        LogMessage("ConnectPort", "Failed to open port " + comPort);
                        throw new Exception("Failed to open port");
                    }
                }
                if (serialPort.IsOpen)
                {
                    LogMessage("ConnectPort", "Opened port " + comPort);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                if (serialPort.IsOpen)
                {
                    serialPort.Close();
                }
            }
        }


        internal static void ConnectHardware()
        {

            LogMessage("InitialiseHardware", "Establishing connection on port " + comPort);
            try
            {
                if (!serialPort.IsOpen)
                {
                    LogMessage("InitialiseHardware", "Serial port not open " + comPort);
                    try { ConnectPort(); }
                    catch (Exception)
                    {
                        LogMessage("InitialiseHardware", "Failed to open port " + comPort);
                        throw new Exception("Failed to open port");
                    }
                }

                // TODO send initial config and get initial data
                LogMessage("InitialiseHardware", "Checking hardware on port " + comPort);
                string ret = Action("readVersion");  // check if comms OK
                if (ret != null)
                {
                    LogMessage("InitialiseHardware", "Connected to hardware on port " + comPort);
                    connectedState = true;
                }
                else
                {
                    LogMessage("InitialiseHardware", "Failed to connect to hardware on port " + comPort);
                    throw new Exception("Failed to connect to hardware on port");
                }

            }
            catch (Exception e)
            {
                connectedState = false;
                System.Diagnostics.Debug.WriteLine(e.Message);
                if (serialPort.IsOpen)
                {
                    serialPort.Close();
                }
            }
        }


        /// <summary>
        /// Place device initialisation code here
        /// </summary>
        /// <remarks>Called every time a new instance of the driver is created.</remarks>
        internal static void InitialiseHardware()
        {
            // This method will be called every time a new ASCOM client loads your driver
            LogMessage("InitialiseHardware", $"Start.");

            // Make sure that "one off" activities are only undertaken once
            if (runOnce == false)
            {
                LogMessage("InitialiseHardware", $"Starting one-off initialisation.");

                DriverDescription = Focuser.DriverDescription; // Get this device's Chooser description

                LogMessage("InitialiseHardware", $"ProgID: {DriverProgId}, Description: {DriverDescription}");

                connectedState = false; // Initialise connected to false
                utilities = new Util(); //Initialise ASCOM Utilities object
                astroUtilities = new AstroUtils(); // Initialise ASCOM Astronomy Utilities object

                LogMessage("InitialiseHardware", "Completed basic initialisation");

                // TODO one-off hardware setup (check that hardware exists and comms ok)
                ConnectPort();

                LogMessage("InitialiseHardware", $"One-off initialisation complete.");
                runOnce = true; // Set the flag to ensure that this code is not run again
            }
        }

        // PUBLIC COM INTERFACE IFocuserV3 IMPLEMENTATION

        #region Common properties and methods.

        /// <summary>
        /// Displays the Setup Dialogue form.
        /// If the user clicks the OK button to dismiss the form, then
        /// the new settings are saved, otherwise the old values are reloaded.
        /// THIS IS THE ONLY PLACE WHERE SHOWING USER INTERFACE IS ALLOWED!
        /// </summary>
        public static void SetupDialog()
        {
            // Don't permit the setup dialogue if already connected
            if (IsConnected)
                MessageBox.Show("Already connected, just press OK");

            using (SetupDialogForm F = new SetupDialogForm(tl))
            {
                var result = F.ShowDialog();
                if (result == DialogResult.OK)
                {
                    WriteProfile(); // Persist device configuration values to the ASCOM Profile store
                }
            }
        }

        /// <summary>Returns the list of custom action names supported by this driver.</summary>
        /// <value>An ArrayList of strings (SafeArray collection) containing the names of supported actions.</value>
        public static ArrayList SupportedActions
        {
            get
            {
                //LogMessage("SupportedActions Get", "Returning empty ArrayList");
                //return new ArrayList();
                
                ArrayList actions = new ArrayList();
                actions.Add("readVersion");
                actions.Add("readHumidity");
                actions.Add("readPressure");
                actions.Add("readDewpoint");
                actions.Add("emergencyStop");
                actions.Add("setRate");
                actions.Add("setHome");
                actions.Add("setPosition");
                actions.Add("moveRelative");
                actions.Add("jogOut");
                actions.Add("jogIn");
                return actions;
                
            }
        }

        /// <summary>Invokes the specified device-specific custom action.</summary>
        /// <param name="ActionName">A well known name agreed by interested parties that represents the action to be carried out.</param>
        /// <param name="ActionParameters">List of required parameters or an <see cref="String.Empty">Empty String</see> if none are required.</param>
        /// <returns>A string response. The meaning of returned strings is set by the driver author.
        /// <para>Suppose filter wheels start to appear with automatic wheel changers; new actions could be <c>QueryWheels</c> and <c>SelectWheel</c>. The former returning a formatted list
        /// of wheel names and the second taking a wheel name and making the change, returning appropriate values to indicate success or failure.</para>
        /// </returns>
        public static string Action(string actionName, string actionParameters = "0")
        {
            string ret = null;
            LogMessage("Action", actionName);
            switch (actionName)
            {

                case "readVersion":
                    ret = CommandString("Rv");
                    if(ret != null)
                    {
                        if(ret[1] == 'v')
                        {
                            ret = ret.Substring(2, ret.Length - 2);
                        }
                        else
                        {
                            ret = null;
                        }
                    }
                    break;

                case "setRate":
                    CommandBlind("Sr" + actionParameters);
                    break;

                default:
                    LogMessage("Action", $"Action {actionName}, parameters {actionParameters} is not implemented");
                    throw new ActionNotImplementedException("Action " + actionName + " is not implemented by this driver");
            }
            return ret;
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and does not wait for a response.
        /// Optionally, protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="Command">The literal command string to be transmitted.</param>
        /// <param name="Raw">
        /// if set to <c>true</c> the string is transmitted 'as-is'.
        /// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
        /// </param>
        public static void CommandBlind(string command, bool raw = false)
        {
            LogMessage("CommandBlind", command);

            if (!serialPort.IsOpen)
            {
                LogMessage("CommandBlind", "Serial port not open");
            }

            try
            {
                serialPort.WriteLine("[" + command + "]");
            }
            catch (Exception)
            {
                LogMessage("CommandBlind", "SerialPort write timeout");
            }
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and waits for a boolean response.
        /// Optionally, protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="Command">The literal command string to be transmitted.</param>
        /// <param name="Raw">
        /// if set to <c>true</c> the string is transmitted 'as-is'.
        /// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
        /// </param>
        /// <returns>
        /// Returns the interpreted boolean response received from the device.
        /// </returns>
        public static bool CommandBool(string command, bool raw = false)
        {
            CheckConnected("CommandBool");
            // TODO The optional CommandBool method should either be implemented OR throw a MethodNotImplementedException
            // If implemented, CommandBool must send the supplied command to the mount, wait for a response and parse this to return a True or False value

            throw new MethodNotImplementedException($"CommandBool - Command:{command}, Raw: {raw}.");
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and waits for a string response.
        /// Optionally, protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="Command">The literal command string to be transmitted.</param>
        /// <param name="Raw">
        /// if set to <c>true</c> the string is transmitted 'as-is'.
        /// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
        /// </param>
        /// <returns>
        /// Returns the string response received from the device.
        /// </returns>
        public static string CommandString(string command, bool raw = false)
        {

            LogMessage("CommandString", command);

            if (!serialPort.IsOpen)
            {
                LogMessage("CommandString", "Serial port not open");
            }

            newRX = false;

            try
            {
                serialPort.WriteLine("[" + command + "]");
            }
            catch (Exception)
            {
                LogMessage("CommandString", "SerialPort write timeout");
                return null;
            }

            DateTime startTime = DateTime.Now;
            Double elapsedMillis = 0;

            while (!newRX)
            {
                elapsedMillis = ((TimeSpan)(DateTime.Now - startTime)).TotalMilliseconds;
                if (elapsedMillis > responseTimeout)
                {
                    LogMessage("CommandString", "Response timeout");
                    return null;
                }
            }

            return rxString;
            
        }

        /// <summary>
        /// Deterministically release both managed and unmanaged resources that are used by this class.
        /// </summary>
        /// <remarks>
        /// TODO: Release any managed or unmanaged resources that are used in this class.
        /// 
        /// Do not call this method from the Dispose method in your driver class.
        ///
        /// This is because this hardware class is decorated with the <see cref="HardwareClassAttribute"/> attribute and this Dispose() method will be called 
        /// automatically by the  local server executable when it is irretrievably shutting down. This gives you the opportunity to release managed and unmanaged 
        /// resources in a timely fashion and avoid any time delay between local server close down and garbage collection by the .NET runtime.
        ///
        /// For the same reason, do not call the SharedResources.Dispose() method from this method. Any resources used in the static shared resources class
        /// itself should be released in the SharedResources.Dispose() method as usual. The SharedResources.Dispose() method will be called automatically 
        /// by the local server just before it shuts down.
        /// 
        /// </remarks>
        public static void Dispose()
        {
            try { LogMessage("Dispose", $"Disposing of assets and closing down."); } catch { }

            try
            {
                // Clean up the trace logger and utility objects
                tl.Enabled = false;
                tl.Dispose();
                tl = null;
            }
            catch { }

            try
            {
                utilities.Dispose();
                utilities = null;
            }
            catch { }

            try
            {
                astroUtilities.Dispose();
                astroUtilities = null;
            }
            catch { }

            // CUSTOM
            // Release serial port connection
            try
            {
                if (serialPort.IsOpen)
                {
                    serialPort.Close();
                }
            }
            catch { }
        }

        /// <summary>
        /// Set True to connect to the device hardware. Set False to disconnect from the device hardware.
        /// You can also read the property to check whether it is connected. This reports the current hardware state.
        /// </summary>
        /// <value><c>true</c> if connected to the hardware; otherwise, <c>false</c>.</value>
        public static bool Connected
        {
            get
            {
                LogMessage("Connected", $"Get {IsConnected}");
                return IsConnected;
            }
            set
            {
                LogMessage("Connected", $"Set {value}");
                if (value == IsConnected)
                    return;

                if (value)
                {
                    LogMessage("Connected Set", $"Connecting to port {comPort}");

                    // TODO insert connect to the device code here
                    ConnectHardware();
                    
                }
                else
                {
                    LogMessage("Connected Set", $"Disconnecting from port {comPort}");

                    // TODO insert disconnect from the device code here
                    if (serialPort.IsOpen)
                    {
                        if (moving)
                        {
                            Halt();
                        }
                        serialPort.Close();
                    }

                    connectedState = false;
                }
            }
        }

        /// <summary>
        /// Returns a description of the device, such as manufacturer and model number. Any ASCII characters may be used.
        /// </summary>
        /// <value>The description.</value>
        public static string Description
        {
            // TODO customise this device description if required
            get
            {
                LogMessage("Description Get", DriverDescription);
                return DriverDescription;
            }
        }

        /// <summary>
        /// Descriptive and version information about this ASCOM driver.
        /// </summary>
        public static string DriverInfo
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                // TODO customise this driver description if required
                string driverInfo = $"Version: {version.Major}.{version.Minor}";
                LogMessage("DriverInfo Get", driverInfo);
                return driverInfo;
            }
        }

        /// <summary>
        /// A string containing only the major and minor version of the driver formatted as 'm.n'.
        /// </summary>
        public static string DriverVersion
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                string driverVersion = $"{version.Major}.{version.Minor}";
                LogMessage("DriverVersion Get", driverVersion);
                return driverVersion;
            }
        }

        /// <summary>
        /// The interface version number that this device supports.
        /// </summary>
        public static short InterfaceVersion
        {
            // set by the driver wizard
            get
            {
                LogMessage("InterfaceVersion Get", "3");
                return Convert.ToInt16("3");
            }
        }

        /// <summary>
        /// The short name of the driver, for display purposes
        /// </summary>
        public static string Name
        {
            // TODO customise this device name as required
            get
            {
                string name = "McMullenTech Focuser";
                LogMessage("Name Get", name);
                return name;
            }
        }

        #endregion

        #region IFocuser Implementation

        private static int focuserPosition = 0; // Class level variable to hold the current focuser position
        private const int focuserSteps = 37000;

        /// <summary>
        /// True if the focuser is capable of absolute position; that is, being commanded to a specific step location.
        /// </summary>
        internal static bool Absolute
        {
            get
            {
                LogMessage("Absolute Get", true.ToString());
                return true; // This is an absolute focuser
            }
        }

        /// <summary>
        /// Immediately stop any focuser motion due to a previous <see cref="Move" /> method call.
        /// </summary>
        internal static void Halt()
        {
            //LogMessage("Halt", "Not implemented");
            //throw new MethodNotImplementedException("Halt");
            LogMessage("Halt", "Execute");
            CommandBlind("Ms");
            
        }

        /// <summary>
        /// True if the focuser is currently moving to a new position. False if the focuser is stationary.
        /// </summary>
        internal static bool IsMoving
        {
            get
            {
                LogMessage("IsMoving Get", moving.ToString());
                return moving;
            }
        }

        /// <summary>
        /// State of the connection to the focuser.
        /// </summary>
        internal static bool Link
        {
            get
            {
                LogMessage("Link Get", Connected.ToString());
                return Connected; // Direct function to the connected method, the Link method is just here for backwards compatibility
            }
            set
            {
                LogMessage("Link Set", value.ToString());
                Connected = value; // Direct function to the connected method, the Link method is just here for backwards compatibility
            }
        }

        /// <summary>
        /// Maximum increment size allowed by the focuser;
        /// i.e. the maximum number of steps allowed in one move operation.
        /// </summary>
        internal static int MaxIncrement
        {
            get
            {
                LogMessage("MaxIncrement Get", focuserSteps.ToString());
                return focuserSteps; // Maximum change in one move
            }
        }

        /// <summary>
        /// Maximum step position permitted.
        /// </summary>
        internal static int MaxStep
        {
            get
            {
                LogMessage("MaxStep Get", focuserSteps.ToString());
                return focuserSteps; // Maximum extent of the focuser, so position range is 0 to 10,000
            }
        }

        /// <summary>
        /// Moves the focuser by the specified amount or to the specified position depending on the value of the <see cref="Absolute" /> property.
        /// </summary>
        /// <param name="Position">Step distance or absolute position, depending on the value of the <see cref="Absolute" /> property.</param>
        internal static void Move(int Position)
        {
            LogMessage("Move", Position.ToString());
            Action("setRate", "1");
            CommandBlind("Ma" + Position.ToString());
            /*
            waitForMove = true;  // TODO use this if Move command should block until complete
            while (waitForMove)
            {
                // wait until finished move
            }
            */
        }

        /// <summary>
        /// Current focuser position, in steps.
        /// </summary>
        internal static int Position
        {         
            get
            {
                LogMessage("Position Get", "");
                if (hasMoved)
                {
                    string res = CommandString("Rp");
                    if (!moving)
                    {
                        hasMoved = false;
                    }
                    return focuserPosition; 
                }
                else
                {
                    return focuserPosition;
                }
                
            }

            set
            {
                // TODO check if setting position is valid; if so, implement
            }
        }


        /// <summary>
        /// Step size (microns) for the focuser.
        /// </summary>
        internal static double StepSize
        {
            get
            {
                //LogMessage("StepSize Get", "Not implemented");
                //throw new PropertyNotImplementedException("StepSize", false);
                return 1.0; // using 1 step = 1um, conversion is handled by hardware
            }
        }

        /// <summary>
        /// The state of temperature compensation mode (if available), else always False.
        /// </summary>
        internal static bool TempComp
        {
            get
            {
                LogMessage("TempComp Get", false.ToString());
                return false;
            }
            set
            {
                LogMessage("TempComp Set", "Not implemented");
                throw new PropertyNotImplementedException("TempComp", false);
            }
        }

        /// <summary>
        /// True if focuser has temperature compensation available.
        /// </summary>
        internal static bool TempCompAvailable
        {
            get
            {
                LogMessage("TempCompAvailable Get", false.ToString());
                return false; // Temperature compensation is not available in this driver
            }
        }

        /// <summary>
        /// Current ambient temperature in degrees Celsius as measured by the focuser.
        /// </summary>
        internal static double Temperature
        {
            get
            {
                CommandString("Rt");
                return temperature;

            }
        }

        #endregion

        #region Private properties and methods
        // Useful methods that can be used as required to help with driver development

        /// <summary>
        /// Returns true if there is a valid connection to the driver hardware
        /// </summary>
        private static bool IsConnected
        {
            get
            {
                // TODO check that the driver hardware connection exists and is connected to the hardware
                return connectedState;
            }
        }

        /// <summary>
        /// Use this function to throw an exception if we aren't connected to the hardware
        /// </summary>
        /// <param name="message"></param>
        private static void CheckConnected(string message)
        {
            if (!IsConnected)
            {
                throw new NotConnectedException(message);
            }
        }

        /// <summary>
        /// Read the device configuration from the ASCOM Profile store
        /// </summary>
        internal static void ReadProfile()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "Focuser";
                tl.Enabled = Convert.ToBoolean(driverProfile.GetValue(DriverProgId, traceStateProfileName, string.Empty, traceStateDefault));
                comPort = driverProfile.GetValue(DriverProgId, comPortProfileName, string.Empty, comPortDefault);
            }
        }

        /// <summary>
        /// Write the device configuration to the  ASCOM  Profile store
        /// </summary>
        internal static void WriteProfile()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "Focuser";
                driverProfile.WriteValue(DriverProgId, traceStateProfileName, tl.Enabled.ToString());
                driverProfile.WriteValue(DriverProgId, comPortProfileName, comPort.ToString());
            }
        }

        /// <summary>
        /// Log helper function that takes identifier and message strings
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="message"></param>
        internal static void LogMessage(string identifier, string message)
        {
            tl.LogMessageCrLf(identifier, message);
        }

        /// <summary>
        /// Log helper function that takes formatted strings and arguments
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        internal static void LogMessage(string identifier, string message, params object[] args)
        {
            var msg = string.Format(message, args);
            LogMessage(identifier, msg);
        }
        #endregion
    }
}

