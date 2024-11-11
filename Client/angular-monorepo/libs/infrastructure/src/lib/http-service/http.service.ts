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
  BACKEND_URL =  isDevMode() ? this.origin + '/api/' : `${getProtocol()}://www.crimean-billing.work.gd/api/`;

  public get<T>(method: string) {
    return this.httpClient.get<T>(`${this.BACKEND_URL}${method}`);
  }

  public post<T>(method: string, body?: any, options?: { headers: HttpHeaders } | {[p: string] : string }) {
    return this.httpClient.post<T>(`${this.BACKEND_URL}${method}`, body, options);
  }

  public put<T>(method: string, body: any, options?: { headers: HttpHeaders | {[p: string] : string} }) {
    return this.httpClient.put<T>(`${this.BACKEND_URL}${method}`, body, options);
  }

  public patch<T>(method: string, body: any, options?: { headers: HttpHeaders | {[p: string] : string} }) {
    return this.httpClient.patch<T>(`${this.BACKEND_URL}${method}`, body, options);
  }

  public delete(method: string, options?: { headers: HttpHeaders | {[p: string] : string} }) {
    return this.httpClient.delete(`${this.BACKEND_URL}${method}`, options);
  }
}
