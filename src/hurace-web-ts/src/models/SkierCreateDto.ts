export interface SkierCreateDto {
    firstName: string;
    lastName: string;
    dateOfBirth: Date;
    countryId: number;
    genderId: number;
    imageUrl?: string;
}
