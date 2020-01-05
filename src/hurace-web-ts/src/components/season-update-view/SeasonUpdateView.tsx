import React, { useState, useEffect } from 'react';
import { Season } from '../../interfaces/Season';
import { setStateAsync } from '../../common/stateSetter';
import { getSeasonById } from '../../common/api';
import DatePicker from 'react-datepicker';
import 'react-datepicker/dist/react-datepicker.css';
import styled from 'styled-components';
import { DefaultInput } from '../../theme/StyledComponents';
import { UpdateView } from '../shared/UpdateView';

const Fields = styled.div`
    display: grid;
    grid-template-rows: auto auto;
    grid-template-columns: auto auto;
    gap: 15px;
`;

const DateInput = styled(DefaultInput)`
    width: 75px;
    height: 20px;
`;

const FieldLabel = styled.span``;

export const SeasonUpdateView: React.FC<{ seasonId?: number }> = ({
    seasonId
}) => {
    const [season, setSeason] = useState<Season>();
    useEffect(() => {
        if (seasonId !== undefined && season === undefined) {
            setStateAsync(setSeason, getSeasonById(seasonId));
        } else if (season === undefined) {
            setSeason({
                id: -1,
                endDate: new Date(),
                startDate: new Date()
            });
        }
    }, [seasonId, season]);

    const onSave = () => {
    };
    const onCancel = () => {};

    return (
        <UpdateView
            headerText={seasonId ? 'Saison bearbeiten' : 'Saison erstellen'}
            onSave={onSave}
            onCancel={onCancel}
        >
            <Fields>
                <FieldLabel>Startdatum:</FieldLabel>
                <DatePicker
                    dateFormat="dd.MM.yyyy"
                    placeholderText="Saisonstart"
                    selected={season?.startDate}
                    customInput={<DateInput />}
                    onChange={startDate => {
                        if (startDate && season)
                            setSeason({ ...season, startDate });
                    }}
                />
                <FieldLabel>Enddatum:</FieldLabel>
                <DatePicker
                    dateFormat="dd.MM.yyyy"
                    placeholderText="Saisonende"
                    selected={season?.endDate}
                    customInput={<DateInput />}
                    onChange={endDate => {
                        if (endDate && season)
                            setSeason({ ...season, endDate });
                    }}
                />
            </Fields>
        </UpdateView>
    );
};
