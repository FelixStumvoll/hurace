import { FormikErrors } from 'formik';

import { isNullOrUndefined } from 'util';

export const validateSeason = (values: SeasonFormValues) => {
    const errors: FormikErrors<SeasonFormErrors> = {};

    if (isNullOrUndefined(values.startDate))
        errors.startDate = 'Startdatum benötigt';
    if (isNullOrUndefined(values.endDate)) errors.endDate = 'Enddatum benötigt';

    if (values.startDate && values.endDate && values.startDate > values.endDate)
        errors.startDate = 'Startdatum muss vor Enddatum liegen';

    return errors;
};
