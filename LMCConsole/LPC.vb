Public Class LPC

    Enum LPCOperation
        HALT = 0
        ADD = 1
        SUBTRACT = 2
        STORE = 3
        LOAD = 5
        INPUTOUTPUT = 9
        COB = 0
    End Enum

    Const DEFAULT_MEMORY_SIZE = 100

    Class Calculator
        Private Property myValue As Integer

        Sub New()
            Reset()
        End Sub

        ''' <summary>
        ''' Resets the calculator's current value to 0
        ''' </summary>
        ''' <remarks>Private since the LMC/LPC specification doesn't provide a feature to reset it</remarks>
        Friend Sub Reset()
            myValue = 0
        End Sub

        Sub Load(withValue As Integer)
            myValue = withValue
        End Sub

        Sub Add(aValue As Integer)
            myValue += aValue
        End Sub

        Sub Subtract(aValue As Integer)
            myValue -= aValue
        End Sub

        Function value() As Integer
            Return myValue
        End Function

        Function isZero() As Boolean
            Return myValue = 0
        End Function

        Function isPlus() As Boolean
            Return myValue > 0
        End Function

    End Class

    Class InBasket
        Private Property myValue As Integer

        Sub New()
            Reset()
        End Sub

        Friend Sub Reset()
            myValue = 0
        End Sub

        Function value() As Integer
            Return myValue
        End Function

        ''' <summary>
        ''' Lets the outside world put something into the InBasket
        ''' </summary>
        ''' <param name="aValue"></param>
        ''' <remarks></remarks>
        Sub Input(aValue As Integer)
            myValue = aValue
        End Sub
    End Class

    Class OutBasket
        Private Property myValue As Integer

        Sub New()
            Reset()
        End Sub

        Friend Sub Reset()
            myValue = 0
        End Sub

        Function value() As Integer
            Return myValue
        End Function

        Sub Output(aValue As Integer)
            myValue = aValue
        End Sub
    End Class

    Class Mailboxes
        Structure Mailbox
            Property data As Boolean

            Property myValue As Integer

            Property opCode As LPCOperation
            Property operand As Integer

            Friend Sub Reset()
                data = True

                myValue = 0

                opCode = 0
                operand = 0
            End Sub

            ReadOnly Property instruction As Boolean
                Get
                    Return Not data
                End Get
            End Property
        End Structure

        Private myMailboxes() As Mailbox

        ''' <summary>
        ''' Setup the mailboxes with the default memory size of 100
        ''' </summary>
        ''' <remarks></remarks>
        Sub New()
            ReDim myMailboxes(DEFAULT_MEMORY_SIZE)

            Reset()
        End Sub

        ''' <summary>
        ''' Setup the mailboxes with a new memory size
        ''' </summary>
        ''' <param name="size"></param>
        ''' <remarks></remarks>
        Sub New(size As Integer)
            ReDim myMailboxes(size)

            Reset()
        End Sub

        Friend Sub Reset()
            For Each mb In myMailboxes
                mb.reset()
            Next
        End Sub

        Default Property at(anIndex As Integer) As Mailbox
            Get

            End Get
            Set(value As Mailbox)

            End Set
        End Property
    End Class

    Class InstructionLocation
        Private Property nextInstructionLocation As Integer

        Sub New()
            Reset()
        End Sub

        Friend Sub Reset()
            nextInstructionLocation = 0
        End Sub

        Friend Sub MoveToNextInstruction()
            nextInstructionLocation += 1
        End Sub

        Friend Sub SetToLocation(nextInstructionLocation As Integer)
            Me.nextInstructionLocation = nextInstructionLocation
        End Sub

        Function value() As Integer
            Return nextInstructionLocation
        End Function
    End Class

    ''' <summary>
    ''' The little person does the work
    ''' </summary>
    ''' <remarks></remarks>
    Class LittlePerson
        Private myComputer As LPC

        Private currentInstruction As Mailboxes.Mailbox


        Sub New(insideComputer As LPC)
            myComputer = insideComputer
        End Sub

        ''' <summary>
        ''' Fetch the next instruction
        ''' </summary>
        ''' <remarks></remarks>
        Sub Fetch()
            With myComputer
                currentInstruction = .myMailboxes(.myNextInstruction.value())
                If currentInstruction.data Then
                    Throw New Exception("LPC tried to fetch data as an instruction")
                End If
            End With
        End Sub

        Sub Decode()
            Debug.Assert(currentInstruction.instruction, "LPC tried to decode an item of data")
        End Sub

        Sub Execute()

        End Sub
    End Class

    Property myCalculator As Calculator

    Property myInBasket As InBasket

    Property myOutBasket As OutBasket

    Property myMailboxes As Mailboxes

    Property myNextInstruction As InstructionLocation

    ''' <summary>
    ''' Setup an LPC with the default memory size
    ''' </summary>
    ''' <remarks></remarks>
    Sub New()
        Me.New(DEFAULT_MEMORY_SIZE)
    End Sub

    Sub New(withMemorySize As Integer)
        myCalculator = New Calculator
        myInBasket = New InBasket
        myOutBasket = New OutBasket
        myMailboxes = New Mailboxes(withMemorySize)
        myNextInstruction = New InstructionLocation
    End Sub

    Sub Reset()
        myCalculator.Reset()
        myInBasket.reset()
        myOutBasket.Reset()
        myMailboxes.Reset()
        myNextInstruction.Reset()
    End Sub
End Class
