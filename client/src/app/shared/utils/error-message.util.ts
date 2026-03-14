export function getErrorMessage(err: unknown): string {
  if (!err) {
    return 'Unexpected error occurred. Please try again.';
  }

  // Handle string[] from interceptor (400 validation errors)
  if (Array.isArray(err)) {
    return err.join('\n');
  }

  if (typeof err === 'string') {
    return err;
  }

  if (typeof err !== 'object') {
    return 'Unexpected error occurred. Please try again.';
  }

  const httpError = err as {error?: unknown; message?: string; statusText?: string; status?: number};

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

  if (httpError.statusText && httpError.statusText.trim().length > 0) {
    return `${httpError.status}: ${httpError.statusText}`;
  }

  return 'Unexpected error occurred. Please try again.';
}
