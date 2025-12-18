import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { firstValueFrom, Observable } from 'rxjs';
import { Box, BoxCreateDto, BoxUpdateDto, SearchBoxResult } from "../interfaces/box-inteface";
import {environment} from "../../environments/environment.development";

@Injectable({
  providedIn: 'root'
})
export class BoxService {
  boxes: Box[] = [];
  pageSize: number = 10;

  public totalPages: number = 1;
  public currentPage: number = 1;

  private readonly apiUrl = `${environment.apiUrl}/box`;

  constructor(private readonly http: HttpClient) {
    this.initializeData();
  }

  private initializeData(): void {
    // Fire-and-forget: start the async operation without awaiting
    this.get(1).catch(error => {
      console.error('Failed to preload boxes:', error);
    });
  }

  async get(currentPage: number, searchTerm?: string) {
    let url = `${this.apiUrl}?currentPage=${currentPage}&boxesPerPage=${this.pageSize}`;
    if (searchTerm) {
      url = url.concat(`&searchTerm=${encodeURIComponent(searchTerm)}`);
    }

    const call = this.http.get<SearchBoxResult>(url);
    let result = await firstValueFrom(call);
    this.boxes = result.boxes;
    this.currentPage = result.currentPage;
    this.totalPages = result.totalPages;
    return result;
  }

  public getbyId(id: string) {
    return firstValueFrom(this.http.get<Box>(`${this.apiUrl}/${id}`));
  }

  public create(boxCreateDto: BoxCreateDto) {
    return firstValueFrom(this.http.post<Box>(`${this.apiUrl}`, boxCreateDto));
  }

  public update(id: string, boxUpdateDto: BoxUpdateDto) {
    return firstValueFrom(this.http.put<Box>(`${this.apiUrl}/${id}`, boxUpdateDto));
  }

  public delete(id: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/box/${id}`);
  }

  public getColors(): string[] {
    return ['red', 'blue', 'green', 'yellow', 'black', 'white', 'brown', 'grey', 'orange', 'purple', 'pink', 'gold', 'silver', 'bronze', 'copper'];
  }

  public getMaterials(): string[] {
    return ['cardboard', 'plastic', 'wood', 'metal'];
  }
}
