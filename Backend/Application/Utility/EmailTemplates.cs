using System.Text;

namespace Application.Utility;

public static class EmailTemplates
{
    /// <summary>
    /// Generates a styled HTML body for a password reset email.
    /// </summary>
    /// <param name="resetLink">The unique password reset link</param>
    /// <returns>HTML string for email body</returns>
    public static string GetPasswordResetEmailBody(string resetLink, decimal minutes)
    {
        var sb = new StringBuilder();

        sb.AppendLine("<!DOCTYPE html>");
        sb.AppendLine("<html lang='en'>");
        sb.AppendLine("<head>");
        sb.AppendLine("<meta charset='UTF-8'>");
        sb.AppendLine("<meta name='viewport' content='width=device-width, initial-scale=1.0'>");
        sb.AppendLine("<title>Password Reset</title>");
        sb.AppendLine("<style>");
        sb.AppendLine("  body { font-family: Arial, sans-serif; background-color: #f4f4f7; color: #333; margin: 0; padding: 0; }");
        sb.AppendLine("  .container { max-width: 600px; margin: 40px auto; background-color: #ffffff; border-radius: 8px; padding: 30px; box-shadow: 0 2px 8px rgba(0,0,0,0.1); }");
        sb.AppendLine("  h2 { color: #1a1a23; }");
        sb.AppendLine("  p { line-height: 1.6; font-size: 16px; }");
        sb.AppendLine("  .btn { display: inline-block; padding: 12px 24px; background-color: #7c3aed; color: #fff; text-decoration: none; border-radius: 6px; font-weight: bold; margin-top: 20px; }");
        sb.AppendLine("  .btn:hover { background-color: #5b21b6; }");
        sb.AppendLine("  .footer { margin-top: 30px; font-size: 12px; color: #999; text-align: center; }");
        sb.AppendLine("</style>");
        sb.AppendLine("</head>");
        sb.AppendLine("<body>");
        sb.AppendLine("<div class='container'>");
        sb.AppendLine("  <h2>Password Reset Request</h2>");
        sb.AppendLine("  <p>We received a request to reset your password. Click the button below to reset it. This link is valid for the next 20 minutes.</p>");
        sb.AppendLine($"  <a class='btn' href='{resetLink}'>Reset Password</a>");
        sb.AppendLine($"  <p>The link will expire in {minutes} minutes.</p>");
        sb.AppendLine("  <p>If you did not request a password reset, please ignore this email.</p>");
        sb.AppendLine("  <div class='footer'>If you have any questions, contact our support team.</div>");
        sb.AppendLine("</div>");
        sb.AppendLine("</body>");
        sb.AppendLine("</html>");

        return sb.ToString();
    }
}
