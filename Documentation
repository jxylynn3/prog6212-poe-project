
# 1. Documentation
Youtube URL: https://youtu.be/i_-NVYOKdJk
PowerPoint presentation:
[PROG6212_POEpart3.pptx](https://github.com/user-attachments/files/23670484/PROG6212_POEpart3.pptx)

# 2. Brief overview of what was added to the POE in Part 03
A . HR View Enhancements

A new HR “Super User” role was added with full control over user management and system configuration.

HR now exclusively handles creating all user profiles, including their name, surname, email, username, password, and hourly rate.

The HR dashboard was upgraded to allow editing and updating user information at any time.

A full reporting feature was implemented using LINQ to query claims and generate PDF reports/invoices for download.

The previous registration system was removed to ensure HR-controlled account creation, preventing unauthorized sign-ups.

B. Lecturer View Enhancements

A secure login system for lecturers was added.

When submitting a claim, the lecturer’s name, surname, ID, and hourly rate are now pulled directly from the HR database, removing manual entry.

An automatic claim calculation was added, using hours × hourly rate, updating instantly when hours are entered.

A new validation rule prevents a lecturer from claiming more than 180 hours per month, matching the lecturer feedback requirements.

EF Core is now used for all data operations to store claims, users, and approval statuses.

Lecturers can now track all submitted claims and view their progress through Programme Coordinator and Academic Manager approvals.

C. Admin Views (Programme Coordinator & Academic Manager)

Both roles received separate, dedicated dashboards that show only their responsibilities.

Support for custom login/identity was added to ensure all roles authenticate correctly.

Sessions were implemented throughout the system to maintain user roles and prevent unauthorized access.

Strict role-based access control was added to ensure users cannot open pages or actions that do not belong to their role.

D. PowerPoint Requirements

A new presentation was created detailing updates from Part 02 to Part 03, specifically addressing lecturer feedback.

Key design decisions, UI layouts, and core code features were included using screenshots and short explanations.

