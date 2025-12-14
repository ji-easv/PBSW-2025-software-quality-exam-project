import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { firstValueFrom, Observable } from 'rxjs';
import { Box, BoxCreateDto, BoxUpdateDto, PaginatedBoxList } from "../interfaces/box-inteface";

@Injectable({
  providedIn: 'root'
})
export class BoxService {
  boxes: Box[] = [];
  pageSize: string = "10";
  public pageCount?: number;

  private readonly apiUrl = 'http://localhost:5133/box';

  constructor(private readonly http: HttpClient) {
    this.initializeData();
  }

  private initializeData(): void {
    // Fire-and-forget: start the async operation without awaiting
    this.get(1, this.pageSize).catch(error => {
      console.error('Failed to preload boxes:', error);
    });
  }

  async get(currentPage: number, count?: string, searchTerm?: string) {
    const url = `${this.apiUrl}?currentPage=${currentPage}&boxesPerPage=${count}&searchTerm=${searchTerm ?? ''}`;
    const call = this.http.get<PaginatedBoxList>(url);
    let result = await firstValueFrom(call);
    this.boxes = result.boxes;
    this.pageCount = result.pageCount;
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

  public getColours(): string[] {
    return ['red', 'blue', 'green', 'yellow', 'black', 'white', 'brown', 'grey', 'orange', 'purple', 'pink', 'gold', 'silver', 'bronze', 'copper'];
  }

  public getMaterials(): string[] {
    return ['cardboard', 'plastic', 'wood', 'metal'];
  }
}
