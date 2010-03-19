Public Class Gesture
    Implements IWiiMoteGesture

    Private P_InputQueue As Queue(Of Double)

    Public Event GestureMatchFound(ByVal Gesture As IWiiMoteGesture.Gesture) Implements IWiiMoteGesture.GestureMatchFound

    Public Property InputQueue() As Queue(Of Double) Implements IWiiMoteGesture.InputQueue
        Get
            Return P_InputQueue
        End Get
        Set(ByVal value As Queue(Of Double))

        End Set
    End Property

    Public Function ScanForGesture(ByVal InputQueue As System.Collections.Queue) As Boolean Implements IWiiMoteGesture.ScanForGesture

    End Function

    Public Sub Update(ByVal NewInputValue As Double) Implements IWiiMoteGesture.Update

    End Sub

    Public Property Input() As Double Implements IWiiMoteGesture.Input
        Get

        End Get
        Set(ByVal value As Double)

        End Set
    End Property
End Class
