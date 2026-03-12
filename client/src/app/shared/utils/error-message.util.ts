export function getErrorMessage(err: unknown): string {
  if (!err || typeof err !== 'object') {
    return 'Unexpected error occurred. Please try again.';
  }

  const httpError = err as {error?: unknown; message?: string};
  if (typeof httpError.error === 'string' && httpError.error.trim().length > 0) {
    return httpError.error;
  }

  if (httpError.error && typeof httpError.error === 'object') {
    const apiError = httpError.error as {message?: string; title?: string};
    if (apiError.message) {
      return apiError.message;
    }
    if (apiError.title) {
      return apiError.title;
    }
  }

  if (httpError.message && httpError.message.trim().length > 0) {
    return httpError.message;
  }

  return 'Unexpected error occurred. Please try again.';
}
