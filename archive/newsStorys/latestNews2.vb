
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Contensive.BaseClasses

Namespace Contensive.Addons.newsStorys

    Public Class latestNews

        Inherits AddonBaseClass
        '
        '
        Public Overrides Function Execute(ByVal CP As CPBaseClass) As Object
            Dim returnHtml As String = ""
            '
            Try
                Dim header As String = ""
                Dim breif As String = ""
                Dim image As String = ""
                Dim link As String = ""
                Dim sql As String = ""
                Dim layout As CPBlockBaseClass = CP.BlockNew
                Dim blockLayout As CPBlockBaseClass = CP.BlockNew
                Dim tmpHtml As String = ""
                Dim storyCnt As Integer = 0
                Dim histCount As Integer = 0
                Dim exitLoop As Boolean = False
                Dim storyID As Integer = CP.Doc.GetInteger("newsStoryId")
                Dim cs As BaseClasses.CPCSBaseClass = CP.CSNew()
                Dim storyDate As Date = New Date(2000, 6, 9)
                Dim minDate As Date = New Date(2000, 6, 9)
                layout.OpenLayout("Latest News")
                Dim brief As String
                '                

                If cs.Open("story List", "storydate<=" & CP.Db.EncodeSQLDate(Now), "storydate  Desc") Then
                    storyCnt = CP.Doc.GetInteger("numberOfStories")
                    If storyCnt = 0 Then
                        storyCnt = 5
                    End If

                    exitLoop = False

                    Do
                        '
                        histCount += 1
                        header = cs.GetText("Name")
                        storyDate = cs.GetDate("storydate")
                        link = cs.GetText("link")
                        brief = cs.GetText("brief")
                        storyID = cs.GetInteger("ID")
                        '
                        blockLayout.Load(layout.GetInner(".latestNewsUL"))

                        '<a href="#" id="cssTitle">Tissue Recipient and News Float Rider Adam Teller is featured in article on Rose Parade Float</a>
                        If (storyDate <= minDate) Then
                            blockLayout.SetOuter("#ndate", storyDate)
                        Else
                            blockLayout.SetOuter("#ndate", "<div class=""news-date"" id=""ndate"">" & storyDate & "</div>")
                        End If
                        If brief = "" And link <> "" Then
                      
                            blockLayout.SetOuter("#ltag", " <a target=""_blank"" href=""" & link & """ id=""ltag"">" & header & "</a>")
                        Else
                            Dim qs As String
                            qs = CP.Doc.RefreshQueryString
                            link = "?" & CP.Utils.ModifyQueryString(qs, "newsStoryId", storyID)
                            blockLayout.SetOuter("#ltag", " <a href=""/News-Stories?newsId=" & link & """ id=""ltag"">" & header & "</a>")
                        End If


                        'If link = "" Then
                        '    blockLayout.SetOuter("#ltag", header)
                        'Else
                        '    blockLayout.SetOuter("#ltag", " <a href=""#"" id=""ltag"">" & header & "</a>")
                        'End If


                        '
                        tmpHtml &= blockLayout.GetHtml
                        '
                        If storyCnt <> 0 Then
                            If histCount >= storyCnt Then
                                exitLoop = True
                            End If
                        End If
                        '
                        Call cs.GoNext()
                    Loop While cs.OK And (Not exitLoop)
                End If
                Call cs.Close()
                'do
                ' read a table
                ' end do

                layout.SetInner(".latestNewsUL", tmpHtml)

                returnHtml = layout.GetHtml
            Catch ex As Exception
                CP.Site.ErrorReport(ex, "error in multiFormAjaxSample.execute")
            End Try
            Return returnHtml
        End Function
    End Class
End Namespace



