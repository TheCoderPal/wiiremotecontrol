Public Class WiimoteControllerSettings

    Private _Nunchuck_DeadAreaThreshold As Double
    Private _Nunchuck_Sensitivity As Double
    Private _Nunchuck_Accelleration As Double

    Sub New(ByVal Nunchuck_DeadAreaThreshold As Double, _
            ByVal Nunchuck_Sensitivity As Double, _
            ByVal Nunchuck_Accelleration As Double)

        _Nunchuck_DeadAreaThreshold = Nunchuck_DeadAreaThreshold
        _Nunchuck_Sensitivity = Nunchuck_Sensitivity
        _Nunchuck_Accelleration = Nunchuck_Accelleration
    End Sub


End Class
