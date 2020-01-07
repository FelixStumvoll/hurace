import React, { useCallback, useState } from 'react';
import { UpdateViewWrapper } from '../shared/UpdateViewWrapper';
import styled from 'styled-components';
import { DefaultInput } from '../../theme/StyledComponents';
import DatePicker from 'react-datepicker';
import { Gender } from '../../interfaces/Gender';

const Label = styled.span``;

const SkierInput = styled(DefaultInput)`
    /* width: 100px; */
    height: 21px;
`;

export const SkierUpdateView: React.FC<{
    skierId?: number;
}> = props => {
    const [skierId, setSkierId] = useState();
    const [firstname, setFirstname] = useState();
    const [lastname, setLastname] = useState();
    const [dateOfBirth, setDateOfBirth] = useState();
    const [selectedCountry, setSelectedCountry] = useState();
    const [selectedGenderId, setSelectedGenderId] = useState();

    const onSave = useCallback(() => {}, []);
    const onCancel = useCallback(() => {}, []);
    return (
        <UpdateViewWrapper
            headerText={`Rennläufer ${skierId ? 'bearbeiten' : 'erstellen'}`}
            onCancel={onCancel}
            onSave={onSave}
            rowCount={6}
        >
            <Label>Vorname:</Label>
            <SkierInput></SkierInput>
            <Label>Nachname:</Label>
            <SkierInput></SkierInput>
            <Label>Geburtsdatum:</Label>
            <DatePicker
                dateFormat="dd.MM.yyyy"
                placeholderText="Saisonstart"
                selected={new Date()}
                customInput={<SkierInput />}
                onChange={d => {}}
            />
            <Label>Geschlecht:</Label>
            <select></select>
            <Label>Land:</Label>
            <select></select>
            <Label>Disziplinen:</Label>
        </UpdateViewWrapper>
    );
};
