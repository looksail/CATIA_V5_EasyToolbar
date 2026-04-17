<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。  
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Test1 = New System.Windows.Forms.Button()
        Me.Test2 = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Test1
        '
        Me.Test1.Location = New System.Drawing.Point(32, 29)
        Me.Test1.Name = "Test1"
        Me.Test1.Size = New System.Drawing.Size(267, 60)
        Me.Test1.TabIndex = 0
        Me.Test1.Text = "Test1:ShowActiveDocName"
        Me.Test1.UseVisualStyleBackColor = True
        '
        'Test2
        '
        Me.Test2.Location = New System.Drawing.Point(32, 106)
        Me.Test2.Name = "Test2"
        Me.Test2.Size = New System.Drawing.Size(267, 60)
        Me.Test2.TabIndex = 0
        Me.Test2.Text = "Test2:CreateSimpleCube"
        Me.Test2.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(335, 188)
        Me.Controls.Add(Me.Test2)
        Me.Controls.Add(Me.Test1)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form1"
        Me.ShowIcon = False
        Me.Text = "ET_VBNETCOMDLL64_Test"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Test1 As Button
    Friend WithEvents Test2 As Button
End Class
