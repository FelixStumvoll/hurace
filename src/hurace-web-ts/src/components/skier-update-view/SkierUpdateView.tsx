import React, { useCallback, useState } from 'react';
import { UpdateViewWrapper } from '../shared/UpdateViewWrapper';
import {
    FormFields,
    VerticallyAlignedText,
    FormLabel,
    FormField,
    FormInput,
    FormErrorMessage
} from '../../theme/CustomComponents';
import DatePicker from 'react-datepicker';
import Select from 'react-select';
import {
    updateSkier,
    createSkier,
    updateSkierDisciplines
} from '../../common/api';
import { useHistory } from 'react-router-dom';
import { Formik } from 'formik';
import { FormWrapper } from '../shared/FormWrapper';
import { useSkierForm } from './useSkierForm';
import { SkierFormValues } from '../../types/forms/skier-form';
import { LoadingWrapper } from '../shared/LoadingWrapper';
import { validateSkier } from './validateSkier';
import { useSelectTheme } from './useSelectTheme';

export const SkierUpdateView: React.FC<{
    skierId?: number;
}> = ({ skierId }) => {
    const [
        apiState,
        initialFormValue,
        genders,
        countries,
        disciplines
    ] = useSkierForm(skierId);
    const [saveError, setSaveError] = useState<string>();
    const selectTheme = useSelectTheme();
    const history = useHistory();

    const onSave = useCallback(
        async (values: SkierFormValues, setSubmitting: any) => {
            try {
                let id = skierId;

                let skier = {
                    countryId: values.selectedCountry?.value ?? -1,
                    firstName: values.firstName,
                    lastName: values.lastName,
                    dateOfBirth: values.dateOfBirth,
                    genderId: values.selectedGender?.value ?? -1,
                    imageUrl: values.imageUrl,
                    retired: values.retired
                };

                if (id) await updateSkier({ id, ...skier });
                else id = await createSkier(skier);

                await updateSkierDisciplines(
                    id,
                    values.selectedDisciplines!.map(s => s.value)
                );

                setSubmitting(false);
                history.push(`/skier/${skierId}`);
            } catch (error) {
                setSaveError('Fehler beim Speichern des Skiers');
            }

            // eslint-disable-next-line react-hooks/exhaustive-deps
        },
        [history, skierId]
    );
    const onCancel = useCallback(() => {
        history.push(`/skier${skierId ? '/' + skierId : ''}`);
    }, [history, skierId]);

    return (
        <UpdateViewWrapper
            headerText={`Rennfahrer ${skierId ? 'bearbeiten' : 'erstellen'}`}
        >
            <LoadingWrapper
                loading={apiState.loading}
                error={apiState.error}
                errorMessage="Rennläufer konnte nicht geladen werden"
            >
                {initialFormValue && (
                    <Formik
                        enableReinitialize={true}
                        initialValues={initialFormValue}
                        validate={validateSkier}
                        onSubmit={async (values, { setSubmitting }) =>
                            await onSave(values, setSubmitting)
                        }
                    >
                        {({
                            values,
                            errors,
                            touched,
                            handleChange,
                            handleBlur,
                            handleSubmit,
                            isSubmitting,
                            setFieldValue,
                            setFieldTouched
                        }) => (
                            <FormWrapper
                                isSubmitting={isSubmitting}
                                onCancel={onCancel}
                                onSubmit={handleSubmit}
                                error={saveError}
                            >
                                <FormFields rowCount={7}>
                                    <FormField>
                                        <FormLabel> Vorname:</FormLabel>
                                        <FormInput
                                            value={values.firstName}
                                            placeholder="Vorname"
                                            onChange={handleChange}
                                            onBlur={handleBlur}
                                            name="firstName"
                                        />
                                        {touched.firstName &&
                                            errors.firstName && (
                                                <FormErrorMessage>
                                                    {errors.firstName}
                                                </FormErrorMessage>
                                            )}
                                    </FormField>

                                    <FormField>
                                        <FormLabel>Nachname:</FormLabel>
                                        <FormInput
                                            value={values.lastName}
                                            placeholder="Nachname"
                                            onChange={handleChange}
                                            onBlur={handleBlur}
                                            name="lastName"
                                        />
                                        {touched.lastName &&
                                            errors.lastName && (
                                                <FormErrorMessage>
                                                    {errors.lastName}
                                                </FormErrorMessage>
                                            )}
                                    </FormField>

                                    <FormField>
                                        <VerticallyAlignedText>
                                            Geburtsdatum:
                                        </VerticallyAlignedText>
                                        <DatePicker
                                            dateFormat="dd.MM.yyyy"
                                            placeholderText="Geburtsdatum"
                                            selected={values.dateOfBirth}
                                            customInput={<FormInput />}
                                            onChange={date =>
                                                setFieldValue(
                                                    'dateOfBirth',
                                                    date
                                                )
                                            }
                                            onBlur={handleBlur}
                                            name="dateOfBirth"
                                        />
                                        {touched.dateOfBirth &&
                                            errors.dateOfBirth && (
                                                <FormErrorMessage>
                                                    {errors.dateOfBirth}
                                                </FormErrorMessage>
                                            )}
                                    </FormField>

                                    <FormField>
                                        <VerticallyAlignedText>
                                            Geschlecht:
                                        </VerticallyAlignedText>
                                        <Select
                                            theme={selectTheme}
                                            value={values.selectedGender}
                                            options={genders}
                                            noOptionsMessage={() =>
                                                'keine Geschlechter verfügbar'
                                            }
                                            onChange={val =>
                                                setFieldValue(
                                                    'selectedGender',
                                                    val
                                                )
                                            }
                                            onBlur={() =>
                                                setFieldTouched(
                                                    'selectedGender'
                                                )
                                            }
                                            name="selectedGender"
                                        />
                                        {errors.selectedGender &&
                                            touched.selectedGender && (
                                                <FormErrorMessage>
                                                    {errors.selectedGender}
                                                </FormErrorMessage>
                                            )}
                                    </FormField>

                                    <FormField>
                                        <VerticallyAlignedText>
                                            Ruhestand:
                                        </VerticallyAlignedText>
                                        <FormInput
                                            type="checkbox"
                                            checked={values.retired}
                                            onChange={e =>
                                                setFieldValue(
                                                    'retired',
                                                    e.target.checked
                                                )
                                            }
                                            onBlur={handleBlur}
                                            name="retired"
                                        />
                                        {touched.retired && errors.retired && (
                                            <FormErrorMessage>
                                                {errors.retired}
                                            </FormErrorMessage>
                                        )}
                                    </FormField>

                                    <FormField>
                                        <VerticallyAlignedText>
                                            Land:
                                        </VerticallyAlignedText>
                                        <Select
                                            value={values.selectedCountry}
                                            options={countries}
                                            noOptionsMessage={() =>
                                                'keine Länder verfügbar'
                                            }
                                            onChange={val =>
                                                setFieldValue(
                                                    'selectedCountry',
                                                    val
                                                )
                                            }
                                            onBlur={() =>
                                                setFieldTouched(
                                                    'selectedCountry'
                                                )
                                            }
                                            name="selectedCountry"
                                        />
                                        {touched.selectedCountry &&
                                            errors.selectedCountry && (
                                                <FormErrorMessage>
                                                    {errors.selectedCountry}
                                                </FormErrorMessage>
                                            )}
                                    </FormField>

                                    <FormField>
                                        <VerticallyAlignedText>
                                            Disziplinen:
                                        </VerticallyAlignedText>
                                        <Select
                                            isMulti={true}
                                            value={values.selectedDisciplines}
                                            options={disciplines}
                                            noOptionsMessage={() =>
                                                'keine Disziplinen verfügbar'
                                            }
                                            onChange={val =>
                                                setFieldValue(
                                                    'selectedDisciplines',
                                                    val
                                                )
                                            }
                                            onBlur={handleBlur}
                                            name="selectedDisciplines"
                                        />

                                        {touched.selectedDisciplines &&
                                            errors.selectedDisciplines && (
                                                <FormErrorMessage>
                                                    {errors.selectedDisciplines}
                                                </FormErrorMessage>
                                            )}
                                    </FormField>
                                    <FormField>
                                        <VerticallyAlignedText>
                                            Bild-Url:
                                        </VerticallyAlignedText>
                                        <FormInput
                                            value={values.imageUrl}
                                            placeholder="Bild-Url"
                                            onChange={handleChange}
                                            onBlur={handleBlur}
                                            name="imageUrl"
                                        />
                                        {touched.imageUrl &&
                                            errors.imageUrl && (
                                                <FormErrorMessage>
                                                    {errors.imageUrl}
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
