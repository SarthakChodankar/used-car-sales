export function apiErrorMessage(err: unknown): string {
  const e = err as {
    status?: number;
    error?: { errors?: Record<string, string[]>; detail?: string; title?: string };
  };

  if (e?.status === 0) return 'Cannot reach the server. Please check that the API is running.';
  if (e?.error?.errors) return Object.values(e.error.errors).flat().join(' ');
  if (e?.error?.detail) return e.error.detail;
  return e?.error?.title ?? 'Something went wrong. Please try again.';
}