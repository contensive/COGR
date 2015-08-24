Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Contensive.BaseClasses

Namespace Contensive.Addons.COGR

    Public Class pubArchiveClass
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
                Dim cs As CPCSBaseClass = CP.CSNew()

                layout.OpenLayout("Publication Stories")
                '                

                If cs.Open("Publications", "archive = 1 and pubDate <=" & CP.Db.EncodeSQLDate(Now), "pubDate Desc") Then
                    storyCnt = CP.Doc.GetInteger("numberOfStories")

                    exitLoop = False
                    tmpHtml = cs.GetAddLink()

                    Do
                        '
                        histCount += 1
                        header = cs.GetEditLink() & cs.GetText("Name")
                        breif = cs.GetText("brief")
                        image = cs.GetText("storyImage")
                        link = cs.GetText("link")
                        '
                        blockLayout.Load(layout.GetOuter(".newsArticle"))

                        If image = "" Then
                            blockLayout.SetOuter("#cssThumImg", image)
                        Else
                            blockLayout.SetInner("#cssThumImg", "<img src=""" & CP.Site.FilePath & image & """ alt="""" />")
                        End If

                        '<a href="#" id="cssTitle">Tissue Recipient and News Float Rider Adam Teller is featured in article on Rose Parade Float</a>

                        blockLayout.SetOuter("#cssTitle", header)
                        'If link = "" Then
                        '    blockLayout.SetOuter("#cssTitle", header)
                        'Else
                        '    blockLayout.SetOuter("#cssTitle", "<a target=""_blank"" href=""" & link & """ id=""cssTitle"">" & header & "</a>")
                        'End If
                        blockLayout.SetInner("#cssText", breif)
                        If link = "" Then
                            blockLayout.SetOuter("#articleURL", "")
                        Else
                            blockLayout.SetInner("#articleURL", "<a target=""_blank"" href=""" & link & """>Read More</a>")
                        End If
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

                layout.SetInner(".newsFeedBox", tmpHtml)

                returnHtml = layout.GetHtml
            Catch ex As Exception
                CP.Site.ErrorReport(ex, "error in multiFormAjaxSample.execute")
            End Try
            Return returnHtml
        End Function
    End Class
End Namespace




