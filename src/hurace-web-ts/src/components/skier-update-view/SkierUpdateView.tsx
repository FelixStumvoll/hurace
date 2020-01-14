import React, { useCallback, useState, useEffect, useContext } from 'react';
import { UpdateViewWrapper } from '../shared/UpdateViewWrapper';
import { ThemeContext } from 'styled-components';
import {
    FormFields,
    VerticallyAlignedText,
    FormLabel,
    FormField,
    FormInput,
    FormErrorMessage
} from '../../theme/CustomComponents';
import DatePicker from 'react-datepicker';
import Select, { Theme } from 'react-select';
import {
    getAllCountries,
    getAllGenders,
    getAllDisciplines,
    getSkierById,
    getDisciplinesForSkier,
    updateSkier,
    createSkier,
    updateSkierDisciplines
} from '../../common/api';
import { SelectValue } from '../../interfaces/SelectValue';
import { isNullOrEmpty } from '../../common/stringFunctions';
import { isNullOrUndefined } from 'util';
import { useHistory } from 'react-router-dom';
import { Formik, FormikErrors } from 'formik';
import { FormWrapper } from '../shared/FormWrapper';
import { SkierUpdateDto } from '../../models/SkierUpdateDto';
import { SkierCreateDto } from '../../models/SkierCreateDto';

type SkierFormValues = {
    firstName: string;
    lastName: string;
    dateOfBirth: Date;
    selectedCountry?: SelectValue;
    selectedGender?: SelectValue;
    retired: boolean;
    selectedDisciplines: SelectValue[];
    imageUrl?: string;
};

type SkierFormErrors = {
    firstName: string;
    lastName: string;
    dateOfBirth: string;
    selectedCountry: string;
    selectedGender: string;
    imageUrl: string;
};

export const SkierUpdateView: React.FC<{
    skierId?: number;
}> = ({ skierId }) => {
    //#region state

    const history = useHistory();

    const [initialFormValue, setInitialFormValue] = useState<SkierFormValues>();
    const [countries, setCountries] = useState<SelectValue[]>();
    const [genders, setGenders] = useState<SelectValue[]>();
    const [disciplines, setDisciplines] = useState<SelectValue[]>([]);

    //#endregion

    useEffect(() => {
        const loadData = async () => {
            let countries = (await getAllCountries()).map(c => ({
                label: c.countryName,
                value: c.id
            }));

            let genders = (await getAllGenders()).map(g => ({
                label: g.genderDescription,
                value: g.id
            }));

            let disciplines = (await getAllDisciplines()).map(d => ({
                label: d.disciplineName,
                value: d.id
            }));

            setCountries(countries);
            setGenders(genders);
            setDisciplines(disciplines);
            if (skierId !== undefined) {
                let skier = await getSkierById(skierId!);
                let skierDisciplines = await getDisciplinesForSkier(skierId!);

                if (skier === undefined) return;

                setInitialFormValue({
                    firstName: skier.firstName,
                    lastName: skier.lastName,
                    dateOfBirth: skier.dateOfBirth,
                    imageUrl: skier.imageUrl,
                    retired: skier.retired,
                    selectedCountry: countries?.find(
                        c => c.value === skier.countryId
                    ),
                    selectedDisciplines: disciplines?.filter(d =>
                        skierDisciplines.some(sk => sk.id === d.value)
                    ),
                    selectedGender: genders?.find(
                        g => g.value === skier.genderId
                    )
                });
            } else
                setInitialFormValue({
                    firstName: '',
                    lastName: '',
                    retired: false,
                    imageUrl: '',
                    selectedGender: undefined,
                    dateOfBirth: new Date(),
                    selectedCountry: undefined,
                    selectedDisciplines: []
                });
        };

        loadData();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    const huraceTheme = useContext(ThemeContext);

    const selectTheme = useCallback(
        (theme: Theme) => ({
            ...theme,
            colors: { ...theme.colors, primary: huraceTheme.blue }
        }),
        // eslint-disable-next-line react-hooks/exhaustive-deps
        []
    );

    const onSave = useCallback(async (values: SkierFormValues) => {
        let id = skierId;

        let skier: SkierUpdateDto | SkierCreateDto = {
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

        history.push(`/skier/${id}`);
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);
    const onCancel = useCallback(() => {
        history.push('/skier');
    }, [history]);

    const validateSkier = useCallback((values: SkierFormValues) => {
        const errors: FormikErrors<SkierFormErrors> = {};

        if (isNullOrEmpty(values.firstName))
            errors.firstName = 'Vorname benötigt';
        if (isNullOrEmpty(values.lastName))
            errors.lastName = 'Nachname benötigt';
        if (isNullOrUndefined(values.dateOfBirth))
            errors.dateOfBirth = 'Geburtsdatum benötigt';
        if (isNullOrUndefined(values.selectedCountry))
            errors.selectedCountry = 'Land benötigt';
        if (isNullOrUndefined(values.selectedGender))
            errors.selectedGender = 'Geschlecht benötigt';

        return errors;
    }, []);

    return (
        <UpdateViewWrapper
            headerText={`Rennfahrer ${skierId ? 'bearbeiten' : 'erstellen'}`}
        >
            {!initialFormValue ? (
                <div></div>
            ) : (
                <Formik
                    enableReinitialize={true}
                    initialValues={initialFormValue}
                    validate={validateSkier}
                    onSubmit={async (values, { setSubmitting }) => {
                        await onSave(values);
                        setSubmitting(false);
                    }}
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
                                    {touched.firstName && errors.firstName && (
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
                                    {touched.lastName && errors.lastName && (
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
                                            setFieldValue('dateOfBirth', date)
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
                                            setFieldValue('selectedGender', val)
                                        }
                                        onBlur={() =>
                                            setFieldTouched('selectedGender')
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
                                            setFieldTouched('selectedCountry')
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
                                    {touched.imageUrl && errors.imageUrl && (
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
        </UpdateViewWrapper>
    );
};
