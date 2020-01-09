import React, { useCallback, useState, useEffect } from 'react';
import { getSeasonById, createSeason } from '../../../common/api';
import DatePicker from 'react-datepicker';
import 'react-datepicker/dist/react-datepicker.css';
import {
    DefaultInput,
    FormFields,
    VerticallyAlignedText
} from '../../../theme/CustomComponents';
import { UpdateViewWrapper } from '../../shared/UpdateViewWrapper';
import { updateSeason } from '../../../common/api';
import { useHistory } from 'react-router-dom';

export const SeasonUpdateView: React.FC<{ seasonId?: number }> = ({
    seasonId
}) => {
    const [startDate, setStartDate] = useState<Date | null>(new Date());
    const [endDate, setEndDate] = useState<Date | null>(new Date());

    useEffect(() => {
        const loadData = async () => {
            if (!seasonId) return;
            let season = await getSeasonById(seasonId);
            setStartDate(season.startDate);
            setEndDate(season.endDate);
        };

        loadData();
    }, [seasonId]);

    const history = useHistory();

    const seasonValidator = useCallback(
        () => !!startDate && !!endDate && startDate < endDate,
        [endDate, startDate]
    );

    const onSave = useCallback(() => {
        const save = async () => {
            let id = seasonId;

            if (id)
                await updateSeason({
                    id,
                    startDate: startDate!,
                    endDate: endDate!
                });
            else
                id = await createSeason({
                    startDate: startDate!,
                    endDate: endDate!
                });

            history.push(`/season/${id}`);
        };

        save();
    }, [endDate, history, seasonId, startDate]);

    const onCancel = useCallback(
        () => history.push(`/season${seasonId ? `/${seasonId}` : ''}`),
        [history, seasonId]
    );

    return (
        <UpdateViewWrapper
            headerText={seasonId ? 'Saison bearbeiten' : 'Saison erstellen'}
            onSave={onSave}
            onCancel={onCancel}
            canSave={seasonValidator}
        >
            <FormFields rowCount={2}>
                <VerticallyAlignedText>Startdatum:</VerticallyAlignedText>
                <DatePicker
                    dateFormat="dd.MM.yyyy"
                    placeholderText="Saisonstart"
                    selected={startDate}
                    customInput={<DefaultInput />}
                    onChange={setStartDate}
                />
                <VerticallyAlignedText>Enddatum:</VerticallyAlignedText>
                <DatePicker
                    dateFormat="dd.MM.yyyy"
                    placeholderText="Saisonende"
                    selected={endDate}
                    customInput={<DefaultInput />}
                    onChange={setEndDate}
                />
            </FormFields>
        </UpdateViewWrapper>
    );
};
