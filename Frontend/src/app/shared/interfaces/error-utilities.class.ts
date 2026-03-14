export class ErrorUtilitiesClass {
  public static getErrorMessage(error: any, alternateMessage: string = ''): string {
    let message = '';
    if (typeof error === 'string') {
      message = error as string;
    }
    else {
      message = error?.error?.errors?.length > 0 ?
        error.error.errors[0].message :
        error.error.message || error.message || alternateMessage;
    }
    return message;
  }
}
