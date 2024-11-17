import { inject, Injectable, isDevMode } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

function getProtocol () {
  return origin.startsWith('https') ? 'https' : 'http'
}

@Injectable({
  providedIn: 'root'
})
export class HttpService {

  private httpClient = inject(HttpClient);

  private origin = location.origin;
  LOCAL_DOMAIN = 'localhost:8080'
  PROD_DOMAIN = 'crimean-billing.work.gd'
  BACKEND_URL = `${getProtocol()}://${isDevMode() ? this.LOCAL_DOMAIN : this.PROD_DOMAIN}/api/`

  public get<T>(method: string) {
    return this.httpClient.get<T>(`${this.BACKEND_URL}${method}`, { withCredentials: true });
  }

  public post<T>(method: string, body?: any, options?: { headers: HttpHeaders, withCredentials?: boolean } | {[p: string] : string }) {
    if (!options) {
      options = {}
    }
    options.withCredentials = true
    return this.httpClient.post<T>(`${this.BACKEND_URL}${method}`, body, options);
  }

  public put<T>(method: string, body: any, options?: { withCredentials?: boolean, headers: HttpHeaders | {[p: string] : string} }) {
    if (!options) {
      options = {} as unknown as any
    }
    options!.withCredentials = true
    return this.httpClient.put<T>(`${this.BACKEND_URL}${method}`, body, options);
  }

  public patch<T>(method: string, body: any, options?: { withCredentials?: boolean, headers: HttpHeaders | {[p: string] : string} }) {
    if (!options) {
      options = {} as unknown as any
    }
    options!.withCredentials = true
    return this.httpClient.patch<T>(`${this.BACKEND_URL}${method}`, body, options);
  }

  public delete(method: string, options?: { withCredentials?: boolean, headers: HttpHeaders | {[p: string] : string} }) {
    if (!options) {
      options = {} as unknown as any
    }
    options!.withCredentials = true
    return this.httpClient.delete(`${this.BACKEND_URL}${method}`, options);
  }
}
