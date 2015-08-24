Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Contensive.BaseClasses

Namespace Contensive.Addons.COGR
    '
    ' Sample Vb addon
    '
    Public Class calDisplayRemoteMethod

        Inherits AddonBaseClass
        '

        '        

        Public Overrides Function Execute(ByVal CP As CPBaseClass) As Object
            Dim returnHtml As String = ""
            '
            Try
                Dim cal As BaseClasses.CPBlockBaseClass = CP.BlockNew()
                Dim cs As BaseClasses.CPCSBaseClass = CP.CSNew()
                Dim sS As String = ""   '   script string
                'Dim title As String
                Dim Name As String
                Dim EventStartDate As Date = Nothing
                Dim eventStart As String = ""
                Dim EventEndDate As Date = Nothing
                Dim EventDescription As String = ""
                Dim eventEnd As String = ""
                Dim layout As CPBlockBaseClass = CP.BlockNew
                Dim blockLayout As CPBlockBaseClass = CP.BlockNew
                Dim tmpHtml As String = ""
                Dim recordId As Integer = 0
                Dim histCount As Integer = 0



                layout.OpenLayout("Calendar Display")

                '                

                If cs.Open("Calendar Events", "id", ) Then

                    exitLoop = False
                    tmpHtml = cs.GetAddLink()

                    Do
                        '
                        histCount += 1
                        Name = cs.GetEditLink() & cs.GetText("Name")
                        EventStartDate = cs.GetText("EventStartDate")
                        EventEndDate = cs.GetText("EventEndDate")
                        EventDescription = cs.GetText("EventDescription")
                        recordId = cs.GetText("Id")
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
                            blockLayout.SetOuter("#articleURL1", "")
                        Else
                            blockLayout.SetInner("#articleURL1", "<a target=""_blank"" href=""" & link & """>Read More</a>")
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
        '
        Private Function encodeMinDate(source As Date) As Date
            Dim returnDate As Date = source
            If returnDate < #8/7/1990# Then
                returnDate = Date.MinValue
            End If
            Return returnDate
        End Function

        '
        '=====================================================================================
        ' common report for this class
        '=====================================================================================
        '
        Private Sub errorReport(ByVal cp As CPBaseClass, ByVal ex As Exception, ByVal method As String)
            Try
                cp.Site.ErrorReport(ex, "Unexpected error in sampleClass." & method)
            Catch exLost As Exception
                '
                ' stop anything thrown from cp errorReport
                '
            End Try
        End Sub
    End Class
End Namespace
