
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Contensive.BaseClasses

Namespace Contensive.Addons.COGR
    '
    ' Sample Vb addon
    '
    Public Class addEventLargeCal
        Inherits AddonBaseClass
        '

        '        
        Public Overrides Function Execute(ByVal CP As CPBaseClass) As Object
            Dim returnHtml As String
            Dim csNewEvent As BaseClasses.CPCSBaseClass = CP.CSNew()
            Dim csEditEvent As BaseClasses.CPCSBaseClass = CP.CSNew()
            Try
                Dim message As String = String.Empty
                '
                Dim id As Integer
                Dim title As String = CP.Doc.GetText("tl")
                Dim strStartDate As String = CP.Doc.GetText("sd") ' yyyy-mm-dd mm/dd/yyyy
                Dim strEndDate As String = CP.Doc.GetText("ed")
                ' Dim strStartTime As String = CP.Doc.GetText("st")
                ' Dim strEndTime As String = CP.Doc.GetText("et")
                Dim details As String = CP.Doc.GetText("det")
                'Dim eventDescription As String = CP.Doc.GetText("description")
                '

                ' Call csNewEvent.SetField("StartDate", startDate)
                '
                Dim startDate As Date = CP.Utils.EncodeDate(strStartDate.Replace("-", "/"))
                Dim endDate As Date = CP.Utils.EncodeDate(strEndDate.Replace("-", "/"))
                ' Dim startTime As String = CP.Doc.GetText("st") ''H(:mm)'
                ' Dim endTime As String = CP.Doc.GetText("et") ''H(:mm)'

                ' check if exist record id
                Dim recordId As Integer = CP.Doc.GetInteger("recordid")

                '    
                CP.Utils.AppendLog("CalendarEvent.log", "start date Added:" & strStartDate)
                'Dim csNewEvent As BaseClasses.CPCSBaseClass = CP.CSNew()

                ' pull the information using the email

                ' ccCalendarEvents
                ' timeFormat:  H(:mm)' // uppercase H for 24-hour clock

                Dim existRecord As Boolean = False

                If recordId <> 0 Then
                    existRecord = csNewEvent.Open("Calendar Events", "id=" & recordId)
                    csNewEvent.SetField("Name", title)
                    csNewEvent.SetField("StartDate", startDate.ToString)
                    csNewEvent.SetField("EndDate", endDate.ToString)
                    '
                                    
                    csNewEvent.SetField("details", details)
                End If

                If Not existRecord Then
                    csNewEvent.Insert("Calendar Events")
                    csNewEvent.SetField("Name", title)
                    csNewEvent.SetField("StartDate", startDate.ToString)
                    csNewEvent.SetField("EndDate", endDate.ToString)
                   
                    '
                    csNewEvent.SetField("details", details)
                End If


                '
                Call CP.Cache.Save(cacheNamecalendarEventList, "")


                CP.Utils.AppendLog("CalendarEvent.log", "Event Added:" & title)
                Call csNewEvent.Close()


                returnHtml = "ok"
            Catch ex As Exception
                errorReport(CP, ex, "execute")
                returnHtml = "error"
            End Try
            Return returnHtml
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
