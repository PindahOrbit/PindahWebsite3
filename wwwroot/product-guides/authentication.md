# Authentication & Security

## Executive Summary

Authentication & Security covers how people sign in to Operations, verify their identity, and recover access. The system supports standard email-and-password sign-in, invitations, email confirmation, password reset, and optional two-factor authentication for stronger protection.

---

## Who This Is For

- All users signing in to the system
- Administrators configuring security for the organisation
- New staff accepting invitations

---

## Key Capabilities

- Sign-in and sign-out
- Registration and email confirmation
- Password reset
- Invite-based onboarding
- Two-factor authentication (setup and verification)
- Role-based access after sign-in

---

## Table of Contents

1. [Signing In](#signing-in)
2. [First-Time Registration](#first-time-registration)
3. [Accepting an Invitation](#accepting-an-invitation)
4. [Password Reset](#password-reset)
5. [Two-Factor Authentication](#two-factor-authentication)
6. [Access and Permissions](#access-and-permissions)

---

## Signing In

1. Open your organisation’s Operations web address.
2. Enter your email and password.
3. Complete two-factor verification if prompted.
4. You are taken to the dashboard.

![Screenshot: Login screen — placeholder]

To sign out, open your profile menu in the top-right corner and choose **Logout**.

---

## First-Time Registration

If self-registration is enabled for your organisation:

1. Open the registration page.
2. Enter your details and choose a password.
3. Confirm your email using the link sent to your inbox.
4. Sign in with your new credentials.

![Screenshot: Registration — placeholder]

---

## Accepting an Invitation

When an administrator invites you:

1. Open the invitation link from your email.
2. Complete your profile and set a password.
3. Confirm any required security steps.
4. Sign in to begin work.

![Screenshot: Accept invite — placeholder]

---

## Password Reset

If you forget your password:

1. Open **Password Reset** from the sign-in page.
2. Enter your email address.
3. Follow the reset link in your email.
4. Choose a new password and sign in.

![Screenshot: Password reset — placeholder]

---

## Two-Factor Authentication

Your organisation may require two-factor authentication (2FA) for additional security.

### Setting up 2FA

1. Sign in and open **Two-Factor Setup** when prompted, or follow your administrator’s instructions.
2. Scan the setup code with your authenticator app.
3. Enter the verification code to confirm.
4. Store backup recovery options if offered.

![Screenshot: Two-factor setup — placeholder]

### Signing in with 2FA

After entering your password, enter the current code from your authenticator app on the verification screen.

![Screenshot: Two-factor verify — placeholder]

---

## Access and Permissions

What you see after sign-in depends on your **role** and your organisation’s **active modules**. If you reach a screen you cannot use, you may see a “not authorized” message — contact your administrator to adjust your role or module access.

Administrators can use **Preview as Role** from the profile menu to see the system as another role would — useful for testing access before go-live.

![Screenshot: Not authorized — placeholder]

---

## Platform Note

Sign-in and user identity are handled securely through the platform’s Microsoft-hosted backend services. The sign-in experience is delivered through the Angular web application in your browser.
