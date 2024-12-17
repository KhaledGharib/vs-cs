using System.Windows.Forms;

public static class Prompt
{
    public static string ShowDialog(string text, string caption, string defaultValue = "")
    {
        Form prompt = new Form
        {
            Width = 400,
            Height = 400,
            Text = caption
        };

        Label lblText = new Label { Left = 20, Top = 20, Text = text, Width = 350 };
        TextBox inputBox = new TextBox { Left = 20, Top = 150, Width = 350, Text = defaultValue };
        Button confirmation = new Button { Text = "OK", Left = 270, Top = 100, Width = 100, DialogResult = DialogResult.OK };

        prompt.Controls.Add(lblText);
        prompt.Controls.Add(inputBox);
        prompt.Controls.Add(confirmation);
        prompt.AcceptButton = confirmation;

        return prompt.ShowDialog() == DialogResult.OK ? inputBox.Text : null;
    }
}