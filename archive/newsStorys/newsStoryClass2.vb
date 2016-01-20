
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Contensive.BaseClasses

Namespace Contensive.Addons.newsStorys

    Public Class newsStoryClass
        Inherits AddonBaseClass
        '
        '
        Public Overrides Function Execute(ByVal CP As CPBaseClass) As Object
            Dim returnHtml As String = ""
            '
            Try
                Dim storyID As Integer = CP.Doc.GetInteger("newsStoryId")

                If storyID = 0 Then
                    returnHtml = Getlisting(CP)
                Else
                    returnHtml = GetDetail(CP)
                End If


            Catch ex As Exception
                CP.Site.ErrorReport(ex, "error in multiFormAjaxSample.execute")
            End Try
            Return returnHtml
        End Function
        '
        '
        Private Function Getlisting(ByVal CP As CPBaseClass) As String
            Dim returnHtml As String = ""
            '
            Try
                Dim header As String = ""
                Dim breif As String = ""
                Dim story As String = ""
                Dim image As String = ""
                Dim link As String = ""
                Dim sql As String = ""
                Dim layout As CPBlockBaseClass = CP.BlockNew
                Dim blockLayout As CPBlockBaseClass = CP.BlockNew
                Dim tmpHtml As String = ""
                Dim storyCnt As Integer = 0
                Dim storyID As Integer = CP.Doc.GetInteger("newsStoryId")
                Dim histCount As Integer = 0
                Dim exitLoop As Boolean = False
                Dim cs As CPCSBaseClass = CP.CSNew()

                layout.OpenLayout("News Story List")
                ' 

                If cs.Open("story List", "storydate<=" & CP.Db.EncodeSQLDate(Now), "storydate  Desc") Then
                    If storyID = 0 Then

                        storyCnt = CP.Doc.GetInteger("numberOfStories")

                        exitLoop = False
                        tmpHtml = cs.GetAddLink()

                        Do
                            '
                            histCount += 1
                            header = cs.GetEditLink() & cs.GetText("Name")
                            breif = cs.GetText("brief")
                            image = cs.GetText("imageFilename")
                            link = cs.GetText("link")
                            story = cs.GetText("story")
                            storyID = cs.GetInteger("ID")
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
                                Dim qs As String
                                qs = CP.Doc.RefreshQueryString
                                link = "?" & CP.Utils.ModifyQueryString(qs, "newsStoryId", storyID)
                                blockLayout.SetOuter("#articleURL", "<a target=""_blank"" href=""" & link & """>Read More</a>")

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
                    Else
                        returnHtml = "<p>HelloWorld</p>"

                    End If
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
        '
        ' 
        Private Function GetDetail(ByVal CP As CPBaseClass) As Object
            Dim returnHtml As String = ""
            '
            Try
                Dim header As String = ""
                Dim breif As String = ""
                Dim story As String = ""
                Dim image As String = ""
                Dim link As String = ""
                Dim sql As String = ""
                Dim layout As CPBlockBaseClass = CP.BlockNew
                Dim blockLayout As CPBlockBaseClass = CP.BlockNew
                Dim tmpHtml As String = ""
                Dim storyCnt As Integer = 0
                Dim storyID As Integer = CP.Doc.GetInteger("newsStoryId")
                Dim histCount As Integer = 0
                Dim exitLoop As Boolean = False
                Dim cs As CPCSBaseClass = CP.CSNew()

                '

                '

                layout.OpenLayout("News Story List")
                ' 

                If cs.Open("story List", "ID=" & storyID) Then
                    If storyID <> 0 Then

                        header = cs.GetEditLink() & cs.GetText("Name")
                        breif = cs.GetText("brief")
                        image = cs.GetText("imageFilename")
                        link = cs.GetText("link")
                        story = cs.GetText("story")
                        storyID = cs.GetInteger("ID")
                        '
                        blockLayout.Load(layout.GetOuter(".newsArticle"))

                        If image = "" Then
                            blockLayout.SetOuter("#cssThumImg", image)
                        Else
                            blockLayout.SetInner("#cssThumImg", "<img src=""" & CP.Site.FilePath & image & """ alt="""" />")
                        End If
                        '
                        blockLayout.SetOuter("#cssTitle", header)
                        '
                        blockLayout.SetInner("#cssText", story)
                        '
                        If link = "" Then
                            blockLayout.SetInner("#articleURL", "")
                        Else
                            blockLayout.SetInner("#articleURL", "<a target=""_blank"" href=""" & link & """>Read More</a>")
                        End If
                        Dim qs As String
                        qs = CP.Doc.RefreshQueryString
                        link = "?" & CP.Utils.ModifyQueryString(qs, "newsStoryId", "")
                        blockLayout.SetOuter("#articleRet", "<a href=""" & link & """>Back to news stories</a>")
                        '
                        tmpHtml &= blockLayout.GetHtml
                        '
                  
                   

                    End If
                End If

                Call cs.Close()


                layout.SetInner(".newsFeedBox", tmpHtml)

                returnHtml = layout.GetHtml
            Catch ex As Exception
                CP.Site.ErrorReport(ex, "error in multiFormAjaxSample.execute")
            End Try
            Return returnHtml
        End Function
    End Class
End Namespace


