export interface BookmarkItem {
    id: number;
    title: string;
    url: string;
    notes?: string | null;
    isFavorite: boolean;
    categoryId: number;
}

// For POST: all fields are needed
export interface CreateBookmark {
    title: string;
    url: string;
    notes?: string;
    isFavorite: boolean;
    categoryId: number;
}

// For PATCH: every field is optional (matches your nullable DTO)
export type UpdateBookmark = Partial<CreateBookmark>;