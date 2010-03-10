
Public Interface IWiiMoteGesture
    Enum Gesture
        FlickRight = 0
        FlickLeft = 1
        FlickUp = 2
        FlickDown = 3
    End Enum

    'USE a queue to read off the gesture values.
    'Each time a gesture value is received, the value is added to the queue, the queue
    'is scanned for matching the gesture and returns true if a match is found, else
    'returns nothing.

    Property InputQueue() As Queue(Of Double)


    Event GestureMatchFound(ByVal Gesture As IWiiMoteGesture.Gesture)

    Function ScanForGesture(ByVal InputQueue As Queue) As Boolean

    Sub Update(ByVal NewInputValue As Double)


End Interface
