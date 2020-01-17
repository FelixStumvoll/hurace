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
import { Formik } from 'formik';
import { useSeasonForm } from './useSeasonForm';
import { LoadingWrapper } from '../shared/LoadingWrapper';
import { validateSeason } from './seasonValidator';

export const SeasonUpdateView: React.FC<{ seasonId?: number }> = ({
    seasonId
}) => {
    const [apiState, initialFormValue] = useSeasonForm(seasonId);
    const [saveError, setSaveError] = useState<string>();
    const history = useHistory();

    const onSave = useCallback(
        async (values: SeasonFormValues, setSubmitting: any) => {
            try {
                let id = seasonId;

                let season = {
                    startDate: values.startDate,
                    endDate: values.endDate
                };

                if (id) await updateSeason({ id, ...season });
                else id = await createSeason(season);

                setSubmitting(false);
                history.push(`/season/${id}`);
            } catch (error) {
                setSaveError('Fehler beim Speichern der Saison');
            }
        },
        [history, seasonId]
    );

    const onCancel = useCallback(
        () => history.push(`/season${seasonId ? `/${seasonId}` : ''}`),
        [history, seasonId]
    );

    return (
        <UpdateViewWrapper
            headerText={seasonId ? 'Saison bearbeiten' : 'Saison erstellen'}
        >
            <LoadingWrapper
                loading={apiState.loading}
                error={apiState.error}
                errorMessage="Saison konnte nicht geladen werden"
            >
                {initialFormValue && (
                    <Formik
                        enableReinitialize={true}
                        initialValues={initialFormValue}
                        validate={validateSeason}
                        onSubmit={async (values, { setSubmitting }) =>
                            await onSave(values, setSubmitting)
                        }
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
                                error={saveError}
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
                                        {touched.startDate &&
                                            errors.startDate && (
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
            </LoadingWrapper>
        </UpdateViewWrapper>
    );
};
