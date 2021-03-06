﻿Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Contensive.BaseClasses

Namespace Contensive.Addons.COGR
    '
    ' Sample Vb addon
    '
    Public Class addEventClass
        Inherits AddonBaseClass
        '

        '        
        Public Overrides Function Execute(ByVal CP As CPBaseClass) As Object
            Dim returnHtml As String
            Dim csNewEvent As BaseClasses.CPCSBaseClass = CP.CSNew()
            Try
                Dim message As String = String.Empty
                '
                Dim title As String = CP.Doc.GetText("title")
                Dim strStartDate As String = CP.Doc.GetText("startDateString") ' yyyy-mm-dd mm/dd/yyyy
                Dim strEndDate As String = CP.Doc.GetText("endDateString")
                'Dim eventDescription As String = CP.Doc.GetText("description")
                '

                ' Call csNewEvent.SetField("StartDate", startDate)
                '
                Dim startDate As Date = CP.Utils.EncodeDate(strStartDate.Replace("-", "/"))
                Dim endDate As Date = CP.Utils.EncodeDate(strEndDate.Replace("-", "/"))

                '    
                CP.Utils.AppendLog("CalendarEvent.log", "start date Added:" & strStartDate)
                'Dim csNewEvent As BaseClasses.CPCSBaseClass = CP.CSNew()

                ' pull the information using the email

                ' ccCalendarEvents



                If csNewEvent.Insert("Calendar Events") Then
                    csNewEvent.SetField("Name", title)
                    csNewEvent.SetField("title", title)
                    csNewEvent.SetField("StartDate", startDate.ToString)
                    csNewEvent.SetField("EndDate", endDate.ToString)
                End If
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