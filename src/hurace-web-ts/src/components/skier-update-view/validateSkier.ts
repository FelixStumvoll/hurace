import { SkierFormValues, SkierFormErrors } from '../../types/forms/skier-form';

import { FormikErrors } from 'formik';

import { isNullOrEmpty } from '../../common/stringFunctions';

import { isNullOrUndefined } from 'util';

export const validateSkier = (values: SkierFormValues) => {
    const errors: FormikErrors<SkierFormErrors> = {};

    if (isNullOrEmpty(values.firstName)) errors.firstName = 'Vorname benötigt';
    if (isNullOrEmpty(values.lastName)) errors.lastName = 'Nachname benötigt';
    if (isNullOrUndefined(values.dateOfBirth))
        errors.dateOfBirth = 'Geburtsdatum benötigt';
    if (isNullOrUndefined(values.selectedCountry))
        errors.selectedCountry = 'Land benötigt';
    if (isNullOrUndefined(values.selectedGender))
        errors.selectedGender = 'Geschlecht benötigt';

    return errors;
};
