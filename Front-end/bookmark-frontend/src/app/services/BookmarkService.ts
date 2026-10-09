import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject, signal } from '@angular/core';
import { tap } from 'rxjs';
import { BookmarkItem, CreateBookmark, UpdateBookmark } from '../models/bookmark-item.model';
import { environment } from '../../environments/environment.development';

@Injectable({ providedIn: 'root' })
export class BookmarkService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.baseUrl}`; // change to your API URL

  // State: components read these signals
  private _bookmarks = signal<BookmarkItem[]>([]);
  readonly bookmarks = this._bookmarks.asReadonly();
  readonly loading = signal(false);
  readonly error = signal<string | null>(null);

  // READ (all) - the data load
  load(search?: string, categoryId?: number) {
    this.loading.set(true);
    this.error.set(null);

    let params = new HttpParams();
    if (search) params = params.set('search', search);
    if (categoryId) params = params.set('categoryId', categoryId);

    this.http.get<BookmarkItem[]>(this.apiUrl, { params }).subscribe({
      next: (data) => {
        this._bookmarks.set(data);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Failed to load bookmarks.');
        this.loading.set(false);
      },
    });
  }

  // READ (one)
  getById(id: number) {
    return this.http.get<BookmarkItem>(`${this.apiUrl}/${id}`);
  }

  // CREATE
  create(dto: CreateBookmark) {
    return this.http.post<BookmarkItem>(this.apiUrl, dto).pipe(
      tap((created) => this._bookmarks.update((list) => [...list, created]))
    );
  }

  // UPDATE (partial)
  update(id: number, dto: UpdateBookmark) {
    return this.http.patch<BookmarkItem>(`${this.apiUrl}/${id}`, dto).pipe(
      tap((updated) =>
        this._bookmarks.update((list) => list.map((b) => (b.id === id ? updated : b)))
      )
    );
  }

  // DELETE
  delete(id: number) {
    return this.http.delete<void>(`${this.apiUrl}/${id}`).pipe(
      tap(() => this._bookmarks.update((list) => list.filter((b) => b.id !== id)))
    );
  }

  // Small helper using update()
  toggleFavorite(b: BookmarkItem) {
    return this.update(b.id, { isFavorite: !b.isFavorite });
  }
}