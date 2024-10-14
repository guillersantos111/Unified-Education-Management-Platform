# Unified Education Management Platform/System

A comprehensive desktop application built with C#, .NET, WinForms, and MSSQL Server for managing student enrollments, subject assignments, and user accounts. Designed for both administrative users and students, this system offers an easy-to-use interface with robust data handling, encryption for security, and role-based access.

## Features

### Admin Features:
- **Admin Dashboard**: Track student enrollment and course registrations.
- **Student Subject Assignment**: Admin can assign subjects to students.
- **Account Management**: Admin can create accounts for both students and other admins, with password encryption using Hash and Salt techniques.
- **Student Enrollment**: Admins can enroll students in courses and manage their data.
- **Certificate Requests**: Admin can view and process student requests for certificates.
- **Activity Logs**: Track student login activity with detailed logs.
- **Forgot Password**: Secure password reset functionality using encryption.

### Student Features:
- **Student Dashboard**: View assigned subjects and schedule, along with login activity logs.
- **View Profile**: Students can view and update their personal information.
- **Request Certificate**: Submit a request for official certificates directly through the application.
- **Account Management**: Students can view their account details and update their passwords securely.

## Technologies Used
- **MSSQL Server**: For database management.
- **Entity Framework Core (EF Core)**: For database interaction and ORM.
- **C# and .NET**: For the core logic and backend development.
- **Windows Forms (WinForms)**: For designing the desktop application's user interface.
- **Krypton Toolkit**: For enhanced UI components and modern design.
- **Microsoft.AspNetCore.Cryptography.KeyDerivation & System.Security.Cryptography (Hash & Salt)**: Secure password storage and authentication.
