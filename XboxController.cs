using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Microsoft.Ccr.Core;
using Microsoft.Dss.Core.Attributes;
using Microsoft.Dss.ServiceModel.Dssp;
using Microsoft.Dss.ServiceModel.DsspServiceBase;
using Microsoft.Robotics.Services.GameController.Proxy;
using Microsoft.Robotics.Services.Sample.Lego.Nxt.Battery.Proxy;
using Microsoft.Robotics.Services.Sample.Lego.Nxt.ColorSensor.Proxy;
using Microsoft.Robotics.Services.Sample.Lego.Nxt.Drive.Proxy;
using drive = Microsoft.Robotics.Services.Sample.Lego.Nxt.Drive.Proxy;
using rgb = Microsoft.Robotics.Services.Sample.Lego.Nxt.ColorSensor.Proxy;
using battery = Microsoft.Robotics.Services.Sample.Lego.Nxt.Battery.Proxy;
using gameController = Microsoft.Robotics.Services.GameController.Proxy;

namespace XboxController
{
    [Contract(Contract.Identifier)]
    [DisplayName("XboxController")]
    [Description("XboxController service (no description provided)")]
    internal class XboxControllerService : DsspServiceBase
    {
        [Partner("XInputGamepad", Contract = Microsoft.Robotics.Services.GameController.Proxy.Contract.Identifier,
            CreationPolicy = PartnerCreationPolicy.CreateAlways)] private readonly GameControllerOperations _gamepadPort
                = new GameControllerOperations();

        [Partner("Drive", Contract = Microsoft.Robotics.Services.Sample.Lego.Nxt.Drive.Proxy.Contract.Identifier,
            CreationPolicy = PartnerCreationPolicy.UsePartnerListEntry)] private readonly DriveOperations drivePort =
                new DriveOperations();

        [Partner("Battery", Contract = Microsoft.Robotics.Services.Sample.Lego.Nxt.Battery.Proxy.Contract.Identifier,
            CreationPolicy = PartnerCreationPolicy.UsePartnerListEntry)] private readonly BatteryOperations
            nxtBatteryPort = new BatteryOperations();

        [Partner("RgbSensor",
            Contract = Microsoft.Robotics.Services.Sample.Lego.Nxt.ColorSensor.Proxy.Contract.Identifier,
            CreationPolicy = PartnerCreationPolicy.UseExisting)] private readonly ColorSensorOperations rgbSensorPort =
                new ColorSensorOperations();

        [ServicePort("/XboxController", AllowMultipleInstances = true)] private XboxControllerOperations _mainPort =
            new XboxControllerOperations();

        [ServiceState] private XboxControllerState _state = new XboxControllerState();

        private ColorSensorMode mode;

        public XboxControllerService(DsspServiceCreationPort creationPort)
            : base(creationPort)
        {
        }

        protected override void Start()
        {
            base.Start();

            mode = ColorSensorMode.None;

            var gamepadNotify = new GameControllerOperations();
            _gamepadPort.Subscribe(gamepadNotify);
            Activate(Arbiter.Receive<UpdateAxes>(true, gamepadNotify, GamePadAxisUpdated));
            Activate(Arbiter.Receive<UpdateButtons>(true, gamepadNotify, ButtonUpdateHandler));
        }

        private void GamePadAxisUpdated(UpdateAxes update)
        {
            LogInfo("Right x: " + (update.Body.Rx*.001).ToString(CultureInfo.InvariantCulture));
            LogInfo("Right y: " + (update.Body.Ry * .001).ToString(CultureInfo.InvariantCulture));
            LogInfo("Right z: " + (update.Body.Rz * .001).ToString(CultureInfo.InvariantCulture));
            LogInfo(" Left x: " + (update.Body.X * .001).ToString(CultureInfo.InvariantCulture));
            LogInfo(" Left y: " + (update.Body.Y*.001).ToString(CultureInfo.InvariantCulture));
            LogInfo(" Left z: " + (update.Body.Z * .001).ToString(CultureInfo.InvariantCulture));


            var req = new SetDriveRequest {LeftPower = (update.Body.Rx*.0005), RightPower = (update.Body.Y*-.0005)};

            drivePort.DriveDistance(req);
        }

        private void ButtonUpdateHandler(UpdateButtons update)
        {
            if (update.Body.Pressed[0])
                SpawnIterator(TurnOnSpotLight);
            if (update.Body.Pressed[1])
                SpawnIterator(GetBatteryPower);
        }

        private IEnumerator<ITask> GetBatteryPower()
        {
            yield return Arbiter.Choice(nxtBatteryPort.Get(),
                                        x => LogInfo("Current Voltage: " + x.CurrentBatteryVoltage.ToString(CultureInfo.InvariantCulture)),
                                        x => { });
        }

        private IEnumerator<ITask> TurnOnSpotLight()
        {
            yield return Arbiter.Choice(rgbSensorPort.Get(), setColorSensorState, x => { });
            yield return
                Arbiter.Choice(
                    rgbSensorPort.SetMode(mode),
                    x => LogInfo("I turned the spotlight on."),
                    x => LogError("I pooped my pants!"));
        }

        private void setColorSensorState(ColorSensorState state)
        {
            switch (state.SensorMode)
            {
                case ColorSensorMode.Blue:
                    mode = ColorSensorMode.Red;
                    break;
                case ColorSensorMode.Red:
                    mode = ColorSensorMode.Green;
                    break;
                case ColorSensorMode.Green:
                    mode = ColorSensorMode.Blue;
                    break;
                default:
                    mode = ColorSensorMode.Green;
                    break;
            }
        }
    }
}