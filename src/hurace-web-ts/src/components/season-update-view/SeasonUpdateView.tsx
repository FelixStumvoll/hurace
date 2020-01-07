import React, { useCallback } from 'react';
import { getSeasonById } from '../../common/api';
import DatePicker from 'react-datepicker';
import 'react-datepicker/dist/react-datepicker.css';
import styled from 'styled-components';
import { DefaultInput, FormFields } from '../../theme/CustomComponents';
import { UpdateViewWrapper } from '../shared/UpdateViewWrapper';
import { putSeason } from '../../common/api';
import { useDispatch } from 'react-redux';
import { push } from 'connected-react-router';
import { useStateAsync } from '../../hooks/useStateAsync';

const DateInput = styled(DefaultInput)`
    width: 75px;
    height: 21px;
`;

const FieldLabel = styled.span``;

export const SeasonUpdateView: React.FC<{ seasonId?: number }> = ({
    seasonId
}) => {
    const [season, setSeason] = useStateAsync(async (seasonId: number) => {
        return seasonId
            ? await getSeasonById(seasonId)
            : { id: -1, startDate: new Date(), endDate: new Date() };
    }, seasonId);

    const dispatch = useDispatch();
    const redirectUri = `/season${seasonId ? `/${seasonId}` : ''}`;

    const onSave = useCallback(() => {
        if (!season) return;
        console.log('season', season);
        const save = async () => {
            await putSeason(season!);
            dispatch(push(redirectUri));
        };

        save();
    }, [dispatch, redirectUri, season]);

    const onCancel = useCallback(() => dispatch(push(redirectUri)), [
        dispatch,
        redirectUri
    ]);

    const startDateChange = useCallback(
        (startDate: Date | null) => {
            if (startDate && season) setSeason({ ...season, startDate });
        },
        [season, setSeason]
    );

    const endDateChange = useCallback(
        (endDate: Date | null) => {
            if (endDate && season) setSeason({ ...season, endDate });
        },
        [season, setSeason]
    );
    return (
        <UpdateViewWrapper
            headerText={seasonId ? 'Saison bearbeiten' : 'Saison erstellen'}
            onSave={onSave}
            onCancel={onCancel}
        >
            <FormFields rowCount={2}>
                <FieldLabel>Startdatum:</FieldLabel>
                <DatePicker
                    dateFormat="dd.MM.yyyy"
                    placeholderText="Saisonstart"
                    selected={season?.startDate}
                    customInput={<DateInput />}
                    onChange={startDateChange}
                />
                <FieldLabel>Enddatum:</FieldLabel>
                <DatePicker
                    dateFormat="dd.MM.yyyy"
                    placeholderText="Saisonende"
                    selected={season?.endDate}
                    customInput={<DateInput />}
                    onChange={endDateChange}
                />
            </FormFields>
        </UpdateViewWrapper>
    );
};
