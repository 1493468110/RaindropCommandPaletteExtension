
# Installation

Since this package is not signed by a trusted certificate authority, you need to trust the certificate before installing.

## Step 1: Trust the certificate

1. Right-click the downloaded `.msix` file → **Properties**
2. Switch to the **Digital Signatures** tab
3. Select the signature in the list → click **Details**
4. Click **View Certificate**
5. Click **Install Certificate**
6. Select **Local Machine** → Next
7. Select **Place all certificates in the following store** → click **Browse**
8. Select **Trusted Root Certification Authorities** → OK
9. Next → Finish

## Step 2: Install the extension

Double-click the `.msix` file and click **Install**.
