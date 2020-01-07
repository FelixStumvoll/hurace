import React, { useState, useEffect } from 'react';
import { HeaderCard } from '../shared/HeaderCard';
import styled from 'styled-components';
import { setStateAsync } from '../../common/stateSetter';
import { getSeasonById } from '../../common/api';
import { Season } from '../../interfaces/Season';
import { getDate } from '../../common/timeConverter';

const DetailGrid = styled.div`
    display: grid;
    grid-template-columns: auto auto;
    grid-template-rows: auto auto;
    gap: 10px;
`;

const SeasonLabel = styled.span`
    font-weight: bold;
`;

export const SeasonDetailPanel: React.FC<{ seasonId: number }> = ({
    seasonId
}) => {
    const [season, setSeason] = useState<Season | undefined>(undefined);

    useEffect(() => {
        if (season !== undefined) return;
        setStateAsync(setSeason, getSeasonById(seasonId));
    }, [season, seasonId]);

    return (
        <HeaderCard headerText="Stammdaten">
            {season && (
                <DetailGrid>
                    <SeasonLabel>Startdatum:</SeasonLabel>
                    <span>{getDate(season.startDate)}</span>
                    <SeasonLabel>Enddatum:</SeasonLabel>
                    <span>{getDate(season.endDate)}</span>
                </DetailGrid>
            )}
        </HeaderCard>
    );
};
