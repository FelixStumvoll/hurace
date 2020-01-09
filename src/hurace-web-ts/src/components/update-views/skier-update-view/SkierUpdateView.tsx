import React, { useCallback, useState, useEffect, useContext } from 'react';
import { UpdateViewWrapper } from '../../shared/UpdateViewWrapper';
import styled, { ThemeContext } from 'styled-components';
import {
    DefaultInput,
    FormFields,
    VerticallyAlignedText
} from '../../../theme/CustomComponents';
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
} from '../../../common/api';
import { SelectValue } from '../../../interfaces/SelectValue';
import { isNullOrEmpty } from '../../../common/stringFunctions';
import { isNullOrUndefined } from 'util';
import { useHistory } from 'react-router-dom';

const SkierInput = styled(DefaultInput)`
    width: calc(100% - 12px);
`;

export const SkierUpdateView: React.FC<{
    skierId?: number;
}> = ({ skierId }) => {
    //#region state
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [imageUrl, setImageUrl] = useState('');
    const [dateOfBirth, setDateOfBirth] = useState(new Date());
    const [selectedCountry, setSelectedCountry] = useState<SelectValue>();
    const [selectedGender, setSelectedGender] = useState<SelectValue>();
    const [selectedDisciplines, setSelectedDisciplines] = useState<
        SelectValue[]
    >();
    const history = useHistory();

    const [countries, setCountries] = useState<SelectValue[]>();
    const [genders, setGenders] = useState<SelectValue[]>();
    const [disciplines, setDisciplines] = useState<SelectValue[]>([]);

    //#endregion

    //#region setter

    const firstnameChange = useCallback(
        event => setFirstName(event.target.value),
        [setFirstName]
    );

    const lastnameChange = useCallback(
        event => setLastName(event.target.value),
        [setLastName]
    );

    const imageUrlChange = useCallback(
        event => setImageUrl(event.target.value),
        [setImageUrl]
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

                setFirstName(skier.firstName);
                setLastName(skier.lastName);
                setDateOfBirth(skier.dateOfBirth);
                setImageUrl(skier.imageUrl ?? '');
                setSelectedCountry(
                    countries?.find(c => c.value === skier.countryId)
                );
                setSelectedGender(
                    genders?.find(g => g.value === skier.genderId)
                );
                setSelectedDisciplines(
                    disciplines?.filter(d =>
                        skierDisciplines.some(sk => sk.id === d.value)
                    )
                );
            }
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

    const skierValidator = useCallback(
        () =>
            !isNullOrEmpty(firstName) &&
            !isNullOrEmpty(lastName) &&
            !isNullOrUndefined(dateOfBirth) &&
            !isNullOrUndefined(selectedGender) &&
            !isNullOrUndefined(selectedCountry) &&
            !isNullOrUndefined(selectedDisciplines),
        [
            dateOfBirth,
            firstName,
            lastName,
            selectedCountry,
            selectedDisciplines,
            selectedGender
        ]
    );

    const onSave = useCallback(async () => {
        let id = skierId;

        if (id) {
            await updateSkier({
                id,
                countryId: selectedCountry!.value,
                firstName,
                lastName,
                dateOfBirth,
                genderId: selectedGender!.value,
                imageUrl
            });
        } else {
            id = await createSkier({
                countryId: selectedCountry!.value,
                firstName,
                lastName,
                dateOfBirth,
                genderId: selectedGender!.value,
                imageUrl
            });
        }

        await updateSkierDisciplines(
            id,
            selectedDisciplines!.map(s => s.value)
        );

        history.push(`/skier/${id}`);
    }, [
        dateOfBirth,
        firstName,
        history,
        imageUrl,
        lastName,
        selectedCountry,
        selectedDisciplines,
        selectedGender,
        skierId
    ]);
    const onCancel = useCallback(() => {
        history.push('/skier');
    }, [history]);

    return (
        <UpdateViewWrapper
            headerText={`Rennfahrer ${skierId ? 'bearbeiten' : 'erstellen'}`}
            onCancel={onCancel}
            onSave={onSave}
            canSave={skierValidator}
        >
            <FormFields rowCount={6}>
                <VerticallyAlignedText>Vorname:</VerticallyAlignedText>
                <SkierInput
                    value={firstName}
                    placeholder="Vorname"
                    onChange={firstnameChange}
                ></SkierInput>
                <VerticallyAlignedText>Nachname:</VerticallyAlignedText>
                <SkierInput
                    value={lastName}
                    placeholder="Nachname"
                    onChange={lastnameChange}
                ></SkierInput>
                <VerticallyAlignedText>Geburtsdatum:</VerticallyAlignedText>
                <DatePicker
                    dateFormat="dd.MM.yyyy"
                    placeholderText="Geburtsdatum"
                    selected={dateOfBirth}
                    customInput={<SkierInput />}
                    onChange={dateOfBirthChange}
                />
                <VerticallyAlignedText>Geschlecht:</VerticallyAlignedText>
                <Select
                    theme={selectTheme}
                    value={selectedGender}
                    options={genders}
                    noOptionsMessage={() => 'keine Geschlechter verfügbar'}
                    onChange={genderChange}
                />
                <VerticallyAlignedText>Land:</VerticallyAlignedText>
                <Select
                    value={selectedCountry}
                    options={countries}
                    noOptionsMessage={() => 'keine Länder verfügbar'}
                    onChange={countryChange}
                />
                <VerticallyAlignedText>Disziplinen:</VerticallyAlignedText>
                <Select
                    isMulti={true}
                    value={selectedDisciplines}
                    options={disciplines}
                    noOptionsMessage={() => 'keine Disziplinen verfügbar'}
                    onChange={disciplineChange}
                />
                <VerticallyAlignedText>Bild-Url:</VerticallyAlignedText>
                <SkierInput
                    value={imageUrl}
                    placeholder="Bild-Url"
                    onChange={imageUrlChange}
                ></SkierInput>
            </FormFields>
        </UpdateViewWrapper>
    );
};
