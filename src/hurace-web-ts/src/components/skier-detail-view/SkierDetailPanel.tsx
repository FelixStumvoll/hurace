import React from 'react';
import { Skier } from '../../interfaces/Skier';
import styled from 'styled-components';
import { getDisciplinesForSkier } from '../../common/api';
import { useStateAsync } from '../../hooks/asyncState';
import { getDate } from '../../common/timeConverter';
import { HeaderCard } from '../shared/HeaderCard';

const DetailPanel = styled.div`
    display: flex;
    flex-direction: row;
`;

const StatsPanel = styled.div`
    display: flex;
    flex-direction: column;
    padding: 10px;
`;

const StatsGrid = styled.div`
    display: grid;
    grid-template-columns: auto auto;
    column-gap: 5px;
    grid-template-rows: repeat(3, auto);
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
        <HeaderCard
            contentStyles={{ padding: '0px', width: '100%' }}
            headerText={`${skier.firstName} ${skier.lastName}`}
        >
            <DetailPanel>
                {skier.imageUrl && <img alt="Skier" src={skier.imageUrl} />}
                <StatsPanel>
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
                </StatsPanel>
            </DetailPanel>
        </HeaderCard>
    );
};
