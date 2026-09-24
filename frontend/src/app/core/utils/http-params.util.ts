import { HttpParams } from '@angular/common/http';

export function buildHttpParams(source: object): HttpParams {
  let params = new HttpParams();

  for (const [key, value] of Object.entries(source as Record<string, unknown>)) {
    if (value === null || value === undefined || value === '') {
      continue;
    }

    params = params.set(key, String(value));
  }

  return params;
}
