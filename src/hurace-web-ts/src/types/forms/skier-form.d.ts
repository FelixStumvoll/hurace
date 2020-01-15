export type SkierFormValues = {
    firstName: string;
    lastName: string;
    dateOfBirth: Date;
    selectedCountry?: SelectValue;
    selectedGender?: SelectValue;
    retired: boolean;
    selectedDisciplines: SelectValue[];
    imageUrl?: string;
};

export type SkierFormErrors = {
    firstName: string;
    lastName: string;
    dateOfBirth: string;
    selectedCountry: string;
    selectedGender: string;
    imageUrl: string;
};
