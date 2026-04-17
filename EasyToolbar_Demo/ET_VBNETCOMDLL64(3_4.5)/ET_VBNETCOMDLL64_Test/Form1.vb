
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports ET_VBNETCOMDLL64
Imports INFITF

Public Class Form1

    Dim CATIA As INFITF.Application
    Dim bCatiaConnected As Boolean = False

    Private Function ConnectCatia() As Boolean
        If bCatiaConnected Then Return True

        Try
            CATIA = CType(Marshal.GetActiveObject("CATIA.Application"), INFITF.Application)

            If CATIA Is Nothing Then
                Return False
            End If

            bCatiaConnected = True
            Return True
        Catch ex As System.Runtime.InteropServices.COMException
            MessageBox.Show("Error COM:" & ex.ToString())
        Catch ex As Exception
            MessageBox.Show("Error:" & ex.ToString())
        End Try

        Return False
    End Function

    Private Sub DisConnectCatia()
        If Not bCatiaConnected Then Return

        Marshal.ReleaseComObject(CATIA)
        bCatiaConnected = False
    End Sub

    Private Sub Test1_Click(sender As Object, e As EventArgs) Handles Test1.Click, Test2.Click
        If Not ConnectCatia() Then
            Return
        End If

        Try
            Dim comObj As New ET_VBNETCOMDLL64Class()
            Dim catiaObj As Object = CATIA
            comObj.ShowActiveDocName(catiaObj)
        Catch ex As Exception
            MessageBox.Show("Error:" & ex.ToString())
        End Try

        DisConnectCatia()
    End Sub

    Private Sub Test2_Click(sender As Object, e As EventArgs) Handles Test2.Click
        If Not ConnectCatia() Then
            Return
        End If

        Try
            Dim comObj As New ET_VBNETCOMDLL64Class()
            Dim catiaObj As Object = CATIA
            comObj.CreateSimpleCube(catiaObj)
        Catch ex As Exception
            MessageBox.Show("Error:" & ex.ToString())
        End Try

        DisConnectCatia()
    End Sub

End Class
