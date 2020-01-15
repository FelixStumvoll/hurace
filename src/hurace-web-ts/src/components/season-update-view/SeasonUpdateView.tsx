import React, { useCallback, useState } from 'react';
import { createSeason } from '../../common/api';
import DatePicker from 'react-datepicker';
import 'react-datepicker/dist/react-datepicker.css';
import {
    FormFields,
    VerticallyAlignedText,
    FormField,
    FormInput,
    FormErrorMessage
} from '../../theme/CustomComponents';
import { UpdateViewWrapper } from '../shared/UpdateViewWrapper';
import { updateSeason } from '../../common/api';
import { useHistory } from 'react-router-dom';
import { FormWrapper } from '../shared/FormWrapper';
import { Formik, FormikErrors } from 'formik';
import { SeasonCreateDto } from '../../models/SeasonCreateDto';
import { SeasonUpdateDto } from '../../models/SeasonUpdateDto';
import { isNullOrUndefined } from 'util';
import { useSeasonForm } from './useSeasonForm';

export const SeasonUpdateView: React.FC<{ seasonId?: number }> = ({
    seasonId
}) => {
    const [initialFormValue] = useSeasonForm(seasonId);
    const [apiError, setApiError] = useState<string>();

    const history = useHistory();

    const onSave = useCallback(
        async (values: SeasonFormValues) => {
            try {
                let id = seasonId;

                let season: SeasonCreateDto | SeasonUpdateDto = {
                    startDate: values.startDate,
                    endDate: values.endDate
                };

                if (id) await updateSeason({ id, ...season });
                else id = await createSeason(season);

                history.push(`/season/${id}`);
            } catch (error) {
                setApiError('Fehler beim Speichern der Saison');
            }
        },
        [history, seasonId]
    );

    const onCancel = useCallback(
        () => history.push(`/season${seasonId ? `/${seasonId}` : ''}`),
        [history, seasonId]
    );

    const validateSeason = useCallback((values: SeasonFormValues) => {
        const errors: FormikErrors<SeasonFormErrors> = {};

        if (isNullOrUndefined(values.startDate))
            errors.startDate = 'Startdatum benötigt';
        if (isNullOrUndefined(values.endDate))
            errors.endDate = 'Enddatum benötigt';

        if (
            values.startDate &&
            values.endDate &&
            values.startDate > values.endDate
        )
            errors.startDate = 'Startdatum muss vor Enddatum liegen';

        return errors;
    }, []);

    return (
        <UpdateViewWrapper
            headerText={seasonId ? 'Saison bearbeiten' : 'Saison erstellen'}
        >
            {!initialFormValue ? (
                <div></div>
            ) : (
                <Formik
                    enableReinitialize={true}
                    initialValues={initialFormValue}
                    validate={validateSeason}
                    onSubmit={async (values, { setSubmitting }) => {
                        await onSave(values);
                        setSubmitting(false);
                    }}
                >
                    {({
                        values,
                        errors,
                        touched,
                        handleBlur,
                        handleSubmit,
                        isSubmitting,
                        setFieldValue
                    }) => (
                        <FormWrapper
                            onSubmit={handleSubmit}
                            onCancel={onCancel}
                            isSubmitting={isSubmitting}
                            error={apiError}
                        >
                            <FormFields rowCount={2}>
                                <FormField>
                                    <VerticallyAlignedText>
                                        Startdatum:
                                    </VerticallyAlignedText>
                                    <DatePicker
                                        dateFormat="dd.MM.yyyy"
                                        placeholderText="Saisonstart"
                                        selected={values.startDate}
                                        customInput={<FormInput />}
                                        onChange={date =>
                                            setFieldValue('startDate', date)
                                        }
                                        name="startDate"
                                        onBlur={handleBlur}
                                    />
                                    {touched.startDate && errors.startDate && (
                                        <FormErrorMessage>
                                            {errors.startDate}
                                        </FormErrorMessage>
                                    )}
                                </FormField>
                                <FormField>
                                    <VerticallyAlignedText>
                                        Enddatum:
                                    </VerticallyAlignedText>
                                    <DatePicker
                                        dateFormat="dd.MM.yyyy"
                                        placeholderText="Saisonende"
                                        selected={values.endDate}
                                        customInput={<FormInput />}
                                        onChange={date =>
                                            setFieldValue('endDate', date)
                                        }
                                        name="endDate"
                                        onBlur={handleBlur}
                                    />
                                    {touched.endDate && errors.endDate && (
                                        <FormErrorMessage>
                                            {errors.endDate}
                                        </FormErrorMessage>
                                    )}
                                </FormField>
                            </FormFields>
                        </FormWrapper>
                    )}
                </Formik>
            )}
        </UpdateViewWrapper>
    );
};
