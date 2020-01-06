import React from 'react';
import { Skier } from '../../interfaces/Skier';
import styled from 'styled-components';
import { getDisciplinesForSkier } from '../../common/api';
import { useStateAsync } from '../../hooks/asyncState';
import { getDate } from '../../common/timeConverter';

const DetailPanel = styled.div`
    padding: 10px;
    display: flex;
    flex-direction: column;
`;

const StatsGrid = styled.div`
    display: grid;
    grid-template-columns: auto auto;
    column-gap: 5px;
    grid-template-rows: repeat(3, auto);
`;

const Name = styled.span`
    font-size: 25px;
    font-weight: bold;
`;

const StatLabel = styled.label`
    font-weight: bold;
`;

const DisciplineText = styled.span`
    word-break: break-all;
`;

export const SkierDetailPanel: React.FC<{ skier: Skier }> = ({ skier }) => {
    const [disciplines] = useStateAsync(getDisciplinesForSkier, skier.id);

    return (
        <DetailPanel>
            <Name>
                {skier.firstName} {skier.lastName}
            </Name>
            <StatsGrid>
                <StatLabel>Geschlecht:</StatLabel>
                <span>{skier.gender.genderDescription}</span>
                <StatLabel>Geburtsdatum:</StatLabel>
                <span>{getDate(skier.dateOfBirth)}</span>
                <StatLabel>Land:</StatLabel>
                <span>{skier.country.countryName}</span>
                <StatLabel>Disziplinen:</StatLabel>
            </StatsGrid>
            <DisciplineText>
                {disciplines &&
                    disciplines.map(d => d.disciplineName).toString()}
            </DisciplineText>
        </DetailPanel>
    );
};
