<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.lblAWSAccessKey = New System.Windows.Forms.Label()
        Me.lblSecretKey = New System.Windows.Forms.Label()
        Me.txtAccessKey = New System.Windows.Forms.TextBox()
        Me.txtSecretKey = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(277, 131)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(140, 50)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Save Settings"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'lblAWSAccessKey
        '
        Me.lblAWSAccessKey.AutoSize = True
        Me.lblAWSAccessKey.Font = New System.Drawing.Font("Arial", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAWSAccessKey.Location = New System.Drawing.Point(9, 14)
        Me.lblAWSAccessKey.Name = "lblAWSAccessKey"
        Me.lblAWSAccessKey.Size = New System.Drawing.Size(118, 16)
        Me.lblAWSAccessKey.TabIndex = 1
        Me.lblAWSAccessKey.Text = "AWS Access Key:"
        '
        'lblSecretKey
        '
        Me.lblSecretKey.AutoSize = True
        Me.lblSecretKey.Font = New System.Drawing.Font("Arial", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSecretKey.Location = New System.Drawing.Point(9, 70)
        Me.lblSecretKey.Name = "lblSecretKey"
        Me.lblSecretKey.Size = New System.Drawing.Size(115, 16)
        Me.lblSecretKey.TabIndex = 2
        Me.lblSecretKey.Text = "AWS Secret Key:"
        '
        'txtAccessKey
        '
        Me.txtAccessKey.Location = New System.Drawing.Point(12, 33)
        Me.txtAccessKey.Name = "txtAccessKey"
        Me.txtAccessKey.Size = New System.Drawing.Size(405, 22)
        Me.txtAccessKey.TabIndex = 3
        '
        'txtSecretKey
        '
        Me.txtSecretKey.Location = New System.Drawing.Point(12, 89)
        Me.txtSecretKey.Name = "txtSecretKey"
        Me.txtSecretKey.Size = New System.Drawing.Size(405, 22)
        Me.txtSecretKey.TabIndex = 4
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(427, 193)
        Me.Controls.Add(Me.txtSecretKey)
        Me.Controls.Add(Me.txtAccessKey)
        Me.Controls.Add(Me.lblSecretKey)
        Me.Controls.Add(Me.lblAWSAccessKey)
        Me.Controls.Add(Me.Button1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "AWS Configuration"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Button1 As Button
    Friend WithEvents lblAWSAccessKey As Label
    Friend WithEvents lblSecretKey As Label
    Friend WithEvents txtAccessKey As TextBox
    Friend WithEvents txtSecretKey As TextBox
End Class
