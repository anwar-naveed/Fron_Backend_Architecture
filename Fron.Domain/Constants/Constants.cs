namespace Fron.Domain.Constants;
public struct ApiStatusCodes
{
    // 100 series for failures
    public const short RECORD_ALREADY_EXIST = 100;

    public const short INVALID_USERNAME_PASSWORD = 101;
    public const short RECORD_NOT_FOUND = 102;
    public const short UNABLE_TO_CREATE_RECORD = 103;
    public const short UNABLE_TO_UPDATE_RECORD = 104;
    public const short UNABLE_TO_DELETE_RECORD = 105;
    public const short FAILED = 106;
    public const short FILE_NOT_FOUND = 107;
    public const short FILE_SAVED_FAILED = 108;
    public const short RECORD_CANNOT_BE_UPDATED = 109;
    public const short TEMPLATE_NOT_FOUND = 110;

    // 200 series for success
    public const short RECORD_SAVED_SUCCESSFULLY = 200;

    public const short USER_LOGIN_SUCCESSFULLY = 201;
    public const short RECORD_UPDATED_SUCCESSFULLY = 202;
    public const short RECORD_DELETED_SUCCESSFULLY = 203;
    public const short RECORD_FETCHED_SUCCESSFULLY = 204;
    public const short FILE_SAVED_SUCCESSFULLY = 205;
}

public struct ApiResponseMessages
{
    public const string RECORD_ALREADY_EXIST = "Record already exists.";
    public const string RECORD_SAVED_SUCCESSFULLY = "Record saved successfully.";
    public const string RECORD_UPDATED_SUCCESSFULLY = "Record updated successfully.";
    public const string RECORD_DELETED_SUCCESSFULLY = "Record deleted successfully.";
    public const string SOMETHING_WENT_WRONG = "Something went wrong.";
    public const string INVALID_USERNAME_PASSWORD = "Invalid username or password.";
    public const string INVALID_ROLE = "Invalid role.";
    public const string INVALID_RECORD = "Invalid record.";
    public const string USER_LOGIN_SUCCESSFULLY = "User login successfully.";
    public const string RECORD_NOT_FOUND = "Record not found.";
    public const string UNABLE_TO_CREATE_RECORD = "Unable to create record.";
    public const string UNABLE_TO_UPDATE_RECORD = "Unable to update record.";
    public const string UNABLE_TO_DELETE_RECORD = "Unable to delete record.";
    public const string SUCCESS = "Success.";
    public const string RECORD_FETCHED_SUCCESSFULLY = "Record Fetched Successfully";
    public const string FILE_SAVED_SUCCESSFULLY = "File saved successfully.";
    public const string FILE_NOT_FOUND = "File not found.";
    public const string FILE_SAVED_FAILED = "File saved failed.";
    public const string RECORD_CANNOT_BE_UPDATED = "Record cannot be updated.";
    public const string TEMPLATE_NOT_FOUND = "Template not found.";
}

public struct StoreProcedureNames
{
    public const string GET_USER = "procGetUsers";
    public const string SOURCE_MASTER_INSERT = "espSourceMaster_Insert";
    public const string SOURCE_MASTER_UPDATE = "espSourceMaster_Update";
    public const string EMPLOYEE_HIERARCHY_UPDATE = "HumanResources.uspUpdateEmployeeLogin";
}

public struct SPParams
{
    public const string COMPANY_ID = "@CompanyID";
    public const string SOURCE_ID = "@SourceID";
    public const string SOURCE_NAME = "@SourceName";
    public const string BUSINESS_ENTITY_ID = "@BusinessEntityID";
    public const string ORGANIZATION_NODE = "@OrganizationNode";
    public const string LOGIN_ID = "@LoginID";
    public const string JOB_TITLE = "@JobTitle";
    public const string HIRE_DATE = "@HireDate";
    public const string CURRENT_FLAG = "@CurrentFlag";
}

public struct CommandConstants
{
    public const string EXEC_COMMAND = "exec";
    public const string OUTPUT = "OUTPUT";
}

public struct MagicNumbers
{
    public const short OTP_LENGTH = 4;
    public const short OTP_EXPIRY_DAYS = 1;
    public const short TOKEN_EXPIRY_DAYS = 2;
}

public struct OtpUseCases
{
    public const string REGISTER_OTP = "ROTP";
    public const string LOGIN_OTP = "LOTP";
    public const string FORGOT_OTP = "FOTP";
    public const string VERIFY_EMAIL_OTP = "VOTP";
}

public struct UserRoles
{
    public const string ADMIN_ROLE = "Admin";
}

public struct JwtClaimNames
{
    public const string USER_ID = "UserId";
    public const string EMAIL = "Email";
    public const string USER_NAME = "UserName";
    public const string FIRST_NAME = "FirstName";
    public const string LAST_NAME = "LastName";
    public const string FULL_NAME = "FullName";
    public const string JTI = "JWT ID";
    public const string IAT = "Issued At";
    public const string COMPANY_ID = "CompanyId";
}

public struct FileExtensions
{
    public const string HTML = ".html";
    public const string TEXT = ".text";
    public const string IMAGE = ".image";
    public const string PDF = ".pdf";
    public const string MP4 = ".mp4";
    public const string EXCEL = ".xlsx";
}

public struct FileNames
{
    public const string SPA_DOCUMENT = "SPA.docx";
    public const string VAT_INVOICE = "VATInvoice.docx";
    public const string ROLES_UPLOAD_ERROR = "RolesUploadErrorFile";
    public const string PRODUCT_PDF_FILE = "ProductInventoryPdf";
    public const string PRODUCT_INVENTORY_PDF_FILE = "ProductInventory";
}

public struct EmailTemplates
{
    public const string REGISTER_TEMPLATE = "register";
    public const string LOGIN_TEMPLATE = "login";
    public const string FORGOT_PASSWORD_TEMPLATE = "forgot";
    public const string VERIFY_EMAIL_TEMPLATE = "verify_email";
}

public struct ExceptionMessages
{
    public const string FAILED_TO_START_API = "Error while creating and building generic host builder object";
    public const string UNAUTHORIZED_USER = "User is not authenticated.";
}

public struct Characters
{
    public const string SPACE = "\u0020";
    public const string LEFT_BRACE = "\u007B";
    public const string RIGHT_BRACE = "\u007D";
    public const string DOT = "\u002E";
}

public struct MimeTypes
{
    public const string OCTET = "application/octet-stream";
    public const string PDF = "application/pdf";
}
