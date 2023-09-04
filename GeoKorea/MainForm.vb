Imports CefSharp
Imports CefSharp.WinForms
Imports Newtonsoft.Json

Public Class MainForm

    Private WithEvents browser As ChromiumWebBrowser

    Dim formHeight As Integer
    Dim formWidth As Integer
    Dim jsCode As String
    Dim jsCode2 As String
    Public Property Newtonsoft As Object

    Public Sub New()
        Me.Text = "통계"
    End Sub

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ' CefSharp 초기화
        Dim cefSettings As New CefSettings()
        Cef.Initialize(cefSettings)

        'position 코드 준비
        Dim jsonString = SetPositionData()
        jsCode = "positions = " & jsonString & ";"

        Dim jsonString2 = SetPositionData2()
        jsCode2 = "positions2 = " & jsonString2 & ";"

        ' ChromiumWebBrowser 컨트롤 생성
        browser = New ChromiumWebBrowser(System.IO.Path.Combine(Application.StartupPath, "geometry.html"))

        ' FrameLoadEnd 이벤트 핸들러 추가
        AddHandler browser.FrameLoadEnd, AddressOf browser_FrameLoadEnd

        ' 폼에 ChromiumWebBrowser 컨트롤 추가
        Me.Controls.Add(browser)
        browser.Dock = DockStyle.Fill
    End Sub

    ' 애플리케이션 종료 시 Cef 리소스 정리
    Protected Overrides Sub OnFormClosing(e As FormClosingEventArgs)
        Cef.Shutdown()
        MyBase.OnFormClosing(e)
    End Sub

    Private Async Sub browser_FrameLoadEnd(sender As Object, e As FrameLoadEndEventArgs)
        'html 사이즈 받아옴
        Dim scriptResponse = Await browser.EvaluateScriptAsync("({
            width: document.documentElement.scrollWidth,
            height: document.documentElement.scrollHeight
            });")

        If scriptResponse.Success Then
            Dim dimensions = scriptResponse.Result
            Dim width = Convert.ToInt32(dimensions.width)
            Dim height = Convert.ToInt32(dimensions.height)

            ' html 내 사이즈로 window폼 크기 조정 
            UpdateFormSize(width, height)
        End If

        'positions 정보 json 으로 js에 전달
        Await browser.EvaluateScriptAsync(jsCode)
        Await browser.EvaluateScriptAsync(jsCode2)

    End Sub

    Private Sub UpdateFormSize(width As Integer, height As Integer)
        If Me.InvokeRequired Then
            Me.Invoke(New Action(Of Integer, Integer)(AddressOf UpdateFormSize), width, height)
        Else
            Me.Width = width + 45
            Me.Height = height + 40
        End If
    End Sub
    Private Function SetPositionData() As String
        Dim positionsData As New Dictionary(Of String, List(Of Object)) From
            {
                {"서울특별시", New List(Of Object) From {0, 10, 50, "red"}},
                {"인천광역시", New List(Of Object) From {0, 0, 70, "blue"}},
                {"대구광역시", New List(Of Object) From {0, 0, 20, "green"}},
                {"광주광역시", New List(Of Object) From {-10, 10, 30, "yellow"}},
                {"울산광역시", New List(Of Object) From {0, 0, 14, "purple"}},
                {"부산광역시", New List(Of Object) From {-10, 10, 28, "cyan"}},
                {"대전광역시", New List(Of Object) From {0, 0, 65, "magenta"}},
                {"세종특별자치시", New List(Of Object) From {-10, 10, 84, "lime"}},
                {"제주특별자치도", New List(Of Object) From {0, 0, 15, "black"}},
                {"강원도", New List(Of Object) From {0, 0, 35, "gray"}},
                {"충청북도", New List(Of Object) From {10, -20, 100, "orange"}},
                {"충청남도", New List(Of Object) From {-20, -20, 75, "brown"}},
                {"경상북도", New List(Of Object) From {0, 0, 64, "pink"}},
                {"경상남도", New List(Of Object) From {0, 0, 22, "olive"}},
                {"전라북도", New List(Of Object) From {0, 0, 67, "teal"}},
                {"전라남도", New List(Of Object) From {20, 0, 84, "navy"}},
                {"경기도", New List(Of Object) From {20, 50, 24, "maroon"}}
            }
        Return JsonConvert.SerializeObject(positionsData)
    End Function

    Private Function SetPositionData2() As String
        Dim positionsData As New Dictionary(Of String, List(Of Object)) From
            {
                {"서울특별시", New List(Of Object) From {0, 0, 10, "Chocolate"}},
                {"인천광역시", New List(Of Object) From {0, 0, 20, "Cornflowerblue"}},
                {"대구광역시", New List(Of Object) From {0, 0, 30, "Crimson"}},
                {"광주광역시", New List(Of Object) From {0, 0, 40, "Cornsilk"}},
                {"울산광역시", New List(Of Object) From {0, 0, 50, "Darksalmon"}},
                {"부산광역시", New List(Of Object) From {0, 0, 60, "Darkseagreen"}},
                {"대전광역시", New List(Of Object) From {0, 0, 70, "Darkturquoise"}},
                {"세종특별자치시", New List(Of Object) From {0, 0, 80, "Deeppink"}},
                {"제주특별자치도", New List(Of Object) From {0, 0, 90, "Gold"}},
                {"강원도", New List(Of Object) From {0, 0, 100, "Greenyellow"}},
                {"충청북도", New List(Of Object) From {0, 0, 90, "Goldenrod"}},
                {"충청남도", New List(Of Object) From {0, 0, 80, "Hotpink"}},
                {"경상북도", New List(Of Object) From {0, 0, 70, "Lightblue"}},
                {"경상남도", New List(Of Object) From {0, 0, 60, "Mediumspringgreen"}},
                {"전라북도", New List(Of Object) From {0, 0, 50, "Paleturquoise"}},
                {"전라남도", New List(Of Object) From {0, 0, 40, "Seagreen"}},
                {"경기도", New List(Of Object) From {0, 0, 30, "Steelblue"}}
            }
        Return JsonConvert.SerializeObject(positionsData)
    End Function
End Class
