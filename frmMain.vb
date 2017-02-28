Imports System.Threading

Public Class frmMain

   Private mobjFormBitmap As Bitmap
   Private mobjBitmapGraphics As Graphics
   Private Shared mintFormWidth As Integer
   Private Shared mintFormHeight As Integer
   Private Shared mblnExitProgram As Boolean = False
   Private Shared mdblGravity As Double = 3
   Private mobjBalls As List(Of Ball)

   Private Class Ball

      Public mdblXPosition As Double
      Public mdblYPosition As Double
      Public mdblInitialX As Double
      Public mdblInitialY As Double
      Public mdblInitialXSpeed As Double
      Public mdblInitialYSpeed As Double

      Public Sub Start()

         Dim lngTimeCounter As Long = 0

         Do While Not mblnExitProgram
            mdblXPosition = mdblInitialX + (mdblInitialXSpeed * lngTimeCounter)
            mdblYPosition = mdblInitialY + (mdblInitialYSpeed * lngTimeCounter) + (0.5 * mdblGravity * lngTimeCounter * lngTimeCounter)

            lngTimeCounter += 1

            If mdblYPosition > mintFormHeight - 100 And (mdblInitialYSpeed + mdblGravity * lngTimeCounter) > 0 Then   ' bounce down
               mdblInitialX = mdblXPosition
               mdblInitialY = mdblYPosition
               mdblInitialYSpeed = -(mdblInitialYSpeed + mdblGravity * lngTimeCounter) * 0.7
               lngTimeCounter = 0
            End If

            If mdblYPosition < 20 And (mdblInitialYSpeed + mdblGravity * lngTimeCounter) < 0 Then   ' bounce up
               mdblInitialX = mdblXPosition
               mdblInitialY = mdblYPosition
               mdblInitialYSpeed = -(mdblInitialYSpeed + mdblGravity * lngTimeCounter) * 0.7
               lngTimeCounter = 0
            End If

            If mdblXPosition < 20 And mdblInitialXSpeed < 0 Then   ' bounce left
               mdblInitialX = mdblXPosition
               mdblInitialY = mdblYPosition
               mdblInitialXSpeed = -(mdblInitialXSpeed) * 0.7
               mdblInitialYSpeed = (mdblInitialYSpeed + mdblGravity * lngTimeCounter) * 0.7
               lngTimeCounter = 0
            End If

            If mdblXPosition > mintFormWidth - 20 And mdblInitialXSpeed > 0 Then   ' bounce right
               mdblInitialX = mdblXPosition
               mdblInitialY = mdblYPosition
               mdblInitialXSpeed = -(mdblInitialXSpeed) * 0.7
               mdblInitialYSpeed = (mdblInitialYSpeed + mdblGravity * lngTimeCounter) * 0.7
               lngTimeCounter = 0
            End If

            Thread.Sleep(20)
         Loop

      End Sub

   End Class


   Private Sub frmMain_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

      Static blnDoneOnce As Boolean = False

      If Not blnDoneOnce Then
         blnDoneOnce = True
         mintFormWidth = Me.Width
         mintFormHeight = Me.Height
         mobjFormBitmap = New Bitmap(mintFormWidth, mintFormHeight, Me.CreateGraphics())
         mobjBitmapGraphics = Graphics.FromImage(mobjFormBitmap)
         mobjBitmapGraphics.FillRectangle(Brushes.White, 0, 0, mintFormWidth, mintFormHeight)
         mobjBalls = New List(Of Ball)
      End If

   End Sub

   Private Sub frmMain_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint

      e.Graphics.DrawImage(mobjFormBitmap, 0, 0)

   End Sub

   Private Sub frmMain_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

      mblnExitProgram = True

   End Sub

   Protected Overrides Sub OnPaintBackground(ByVal pevent As System.Windows.Forms.PaintEventArgs)

      ' to remove the flickering

   End Sub

   Private Sub frmMain_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown

      Dim objBall As Ball
      Dim objBallThread As Thread
      Static mintInitialX As Int16 = 0
      Static mintInitialY As Int16 = 0

      If e.Button = Windows.Forms.MouseButtons.Left Then
         If mintInitialX = 0 And mintInitialY = 0 Then
            mintInitialX = e.X
            mintInitialY = e.Y
         Else
            objBall = New Ball
            objBall.mdblInitialX = e.X
            objBall.mdblInitialY = e.Y
            objBall.mdblInitialXSpeed = e.X - mintInitialX
            objBall.mdblInitialYSpeed = e.Y - mintInitialY
            objBallThread = New Thread(AddressOf objBall.Start)
            objBallThread.Start()
            mobjBalls.Add(objBall)

            mintInitialX = 0
            mintInitialY = 0
         End If
      End If

   End Sub

   Private Sub frmMain_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

      Dim blnStarted As Boolean = False

      Select Case e.KeyCode
         Case Keys.Enter
            If Not blnStarted Then
               blnStarted = True
               StartDrawing()
            End If

      End Select

   End Sub

   Private Sub StartDrawing()

      Dim intBallCount As Int16
      Dim objBall As Ball
      Dim objIcon As Icon

      objIcon = New Icon(My.Application.Info.DirectoryPath + "\ball.ico")

      Do While Not mblnExitProgram
         mobjBitmapGraphics.FillRectangle(Brushes.White, 0, 0, mintFormWidth, mintFormHeight)

         intBallCount = 0
         Do While intBallCount <= mobjBalls.Count - 1
            ' draw ball
            objBall = mobjBalls.Item(intBallCount)
            mobjBitmapGraphics.DrawIcon(objIcon, objBall.mdblXPosition, objBall.mdblYPosition)
            intBallCount += 1
         Loop

         Me.Invalidate()
         Application.DoEvents()

         Thread.Sleep(20)
      Loop

   End Sub

End Class




