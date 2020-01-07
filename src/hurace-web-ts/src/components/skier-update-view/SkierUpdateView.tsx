import React, { useCallback, useState, useEffect, useContext } from 'react';
import { UpdateViewWrapper } from '../shared/UpdateViewWrapper';
import styled, { ThemeContext } from 'styled-components';
import { DefaultInput, FormFields } from '../../theme/CustomComponents';
import DatePicker from 'react-datepicker';
import Select, { Theme } from 'react-select';
import {
    getAllCountries,
    getAllGenders,
    getAllDisciplines,
    getSkierById,
    getDisciplinesForSkier
} from '../../common/api';
import { SelectValue } from '../../interfaces/SelectValue';
import { isNullOrEmpty } from '../../common/stringFunctions';
import { isNullOrUndefined } from 'util';

const Label = styled.div`
    height: fit-content;
    margin: auto 0 auto 0;
`;

const SkierInput = styled(DefaultInput)`
    height: 21px;
    width: calc(100% - 12px);
`;

export const SkierUpdateView: React.FC<{
    skierId?: number;
}> = ({ skierId }) => {
    const [dataLoaded, setDataLoaded] = useState(false);
    const [firstname, setFirstname] = useState<string>('');
    const [lastname, setLastname] = useState<string>('');
    const [dateOfBirth, setDateOfBirth] = useState<Date>(new Date());
    const [selectedCountry, setSelectedCountry] = useState<SelectValue>();
    const [selectedGender, setSelectedGender] = useState<SelectValue>();
    const [selectedDisciplines, setSelectedDisciplines] = useState<
        SelectValue[]
    >();

    const [countries, setCountries] = useState<SelectValue[]>();
    const [genders, setGenders] = useState<SelectValue[]>();
    const [disciplines, setDisciplines] = useState<SelectValue[]>();

    const firstnameChange = useCallback(
        event => setFirstname(event.target.value),
        [setFirstname]
    );

    const lastnameChange = useCallback(
        event => setLastname(event.target.value),
        [setLastname]
    );

    const dateOfBirthChange = useCallback(
        dateOfBirth => setDateOfBirth(dateOfBirth),
        [setDateOfBirth]
    );

    const countryChange = useCallback(
        selected => setSelectedCountry(selected),
        [setSelectedCountry]
    );

    const genderChange = useCallback(selected => setSelectedGender(selected), [
        setSelectedGender
    ]);

    const disciplineChange = useCallback(
        selectedOptions => setSelectedDisciplines(selectedOptions),
        [setSelectedDisciplines]
    );

    const [loading, setLoading] = useState(false);

    const loadData = useCallback(async () => {
        setLoading(true);

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

            setFirstname(skier.firstName);
            setLastname(skier.lastName);
            setDateOfBirth(skier.dateOfBirth);
            setSelectedCountry(
                countries?.find(c => c.value === skier.countryId)
            );
            setSelectedGender(genders?.find(g => g.value === skier.genderId));
            setSelectedDisciplines(
                disciplines?.filter(d =>
                    skierDisciplines.some(sk => sk.id === d.value)
                )
            );
        }

        setDataLoaded(true);
        setLoading(false);
    }, [skierId]);

    useEffect(() => {
        if (!loading && !dataLoaded) loadData();
    });

    const onSave = useCallback(() => {}, []);
    const onCancel = useCallback(() => {}, []);

    const huraceTheme = useContext(ThemeContext);

    const selectTheme = useCallback(
        (theme: Theme) => ({
            ...theme,
            colors: { ...theme.colors, primary: huraceTheme.blue }
        }),
        [huraceTheme.blue]
    );

    const skierValidator = useCallback(
        () =>
            !isNullOrEmpty(firstname) &&
            !isNullOrEmpty(lastname) &&
            !isNullOrUndefined(dateOfBirth) &&
            !isNullOrUndefined(selectedGender) &&
            !isNullOrUndefined(selectedCountry) &&
            !isNullOrUndefined(selectedDisciplines),
        [
            dateOfBirth,
            firstname,
            lastname,
            selectedCountry,
            selectedDisciplines,
            selectedGender
        ]
    );

    return (
        <UpdateViewWrapper
            headerText={`Rennläufer ${skierId ? 'bearbeiten' : 'erstellen'}`}
            onCancel={onCancel}
            onSave={onSave}
            canSave={skierValidator}
        >
            <FormFields rowCount={6}>
                <Label>Vorname:</Label>
                <SkierInput
                    value={firstname}
                    placeholder="Vorname"
                    onChange={firstnameChange}
                ></SkierInput>
                <Label>Nachname:</Label>
                <SkierInput
                    value={lastname}
                    placeholder="Nachname"
                    onChange={lastnameChange}
                ></SkierInput>
                <Label>Geburtsdatum:</Label>
                <DatePicker
                    dateFormat="dd.MM.yyyy"
                    placeholderText="Geburtsdatum"
                    selected={dateOfBirth}
                    customInput={<SkierInput />}
                    onChange={dateOfBirthChange}
                />
                <Label>Geschlecht:</Label>
                <Select
                    theme={selectTheme}
                    value={selectedGender}
                    options={genders}
                    noOptionsMessage={() => 'keine Geschlechter verfügbar'}
                    onChange={genderChange}
                />
                <Label>Land:</Label>
                <Select
                    value={selectedCountry}
                    options={countries}
                    noOptionsMessage={() => 'keine Länder verfügbar'}
                    onChange={countryChange}
                />
                <Label>Disziplinen:</Label>
                <Select
                    isMulti={true}
                    value={selectedDisciplines}
                    options={disciplines}
                    noOptionsMessage={() => 'keine Disziplinen verfügbar'}
                    onChange={disciplineChange}
                />
            </FormFields>
        </UpdateViewWrapper>
    );
};
