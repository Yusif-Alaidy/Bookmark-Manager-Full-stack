export interface ICreateBookmark {
    title: string;
    url: string;
    notes?: string;
    isFavorite: boolean;
    categoryId: number;
}
